using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Tracker.TcpServer.RequestHandlerV0_1;
using Tracker.Protocol;
using Tracker.Common.Model;
using Utils;
using Tracker.Common;

namespace Tracker.TcpServer
{
    public class TcpServerV0_1 : ITcpServer
    {
        #region LogProperties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

        public string ProtocolName { get; set; }
        public int Port { get; set; }
        private bool isListening = true;

        private Tracker.Parser.Protocol ProtocolParser { get; set; }

        private Socket listener;
        List<StateObject> ConnectedClientObjs = new List<StateObject>();

        // Thread signal.
        public ManualResetEvent allDone = new ManualResetEvent(false);

        public void Start(string protocolName, IPAddress iPAddress, int port)
        {
            log.InfoFormat("{0}/Start:", _fileNm);

            this.ProtocolName = protocolName;
            this.Port = port;


            log.DebugFormat("{0}/Start: ProtocolName: {1}, Port: {2}", _fileNm, this.ProtocolName, this.Port);

            //Type.GetType("Tracker.Protocol.***Protocol, Tracker.Protocol");
            Type t = Type.GetType("Tracker.Parser." + this.ProtocolName + "Protocol, Tracker.Protocol");
            if (t != null)
            {
                log.InfoFormat("{0}/Start: Known ProtocolName", _fileNm);
                this.ProtocolParser = (Tracker.Parser.Protocol)Activator.CreateInstance(t);
            }
            else
            {
                log.InfoFormat("{0}/Start: Unknown ProtocolName", _fileNm);
                this.ProtocolParser = new Tracker.Parser.UnknownProtocol();
            }
            // Create a TCP/IP socket.
            try
            {
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(new IPEndPoint(iPAddress, port));
                listener.Listen(100);

                ProtocolServerData.SaveData(this.ProtocolName, this.Port, "STARTED", string.Format("Started for: {0}", this.ProtocolParser.GetType().FullName));

                while (isListening)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    log.InfoFormat("{0}/Start: {1}:{2} Waiting for a connection...", _fileNm, this.ProtocolName, this.Port);
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

                // https://msdn.microsoft.com/en-us/library/system.net.sockets.socket.begindisconnect(v=vs.110).aspx
                // Release the socket.

                // Will prevent from connecting new client
                listener.Close(0);

                log.DebugFormat("{0}/Start: Server stop command executed", _fileNm);

                ProtocolServerData.SaveData(this.ProtocolName, this.Port, "CLOSED", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                log.ErrorFormat("/Start: {0}", ex);
                ProtocolServerData.SaveData(this.ProtocolName, this.Port, "ERROR", ex.Message + ex.StackTrace);
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                log.InfoFormat("{0}/AcceptCallback:", _fileNm);

                // Signal the main thread to continue.
                allDone.Set();

                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;
                // Cannot verify the below before EndAccept. On closing the server will make exception. No issues
                //if (listener != null && listener.Connected)
                {
                    Socket clientHandle = listener.EndAccept(ar);

                    //ConnectedClientObjs.Add(clientHandle);
                    log.DebugFormat("{0}/AcceptCallback: ConnectedClients count: {1}", _fileNm, ConnectedClientObjs.Count);


                    // Create the state object.
                    StateObject state = new StateObject()
                    {
                        workSocket = clientHandle,
                        deviceInfo = new DeviceInfo() { TrackerConnectedTime = DateTime.UtcNow }
                    };
                    
                    ConnectedClientObjs.Add(state);

                    log.DebugFormat("{0}/AcceptCallback: {1}, {2}, Connection made from: {3}", _fileNm, ProtocolName, Port, state.workSocket.RemoteEndPoint);

                    state.workSocket.BeginReceive(state.buffer, 0, state.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}/AcceptCallback: {1}", _fileNm, ex);
            }
        }

        public void ReadCallback(IAsyncResult ar)
        {
            log.InfoFormat("{0}/ReadCallback:", _fileNm);

            StateObject state = (StateObject)ar.AsyncState;
            state.lastReadTime = DateTime.UtcNow;

            try
            {
                log.DebugFormat("{0}/ReadCallback: state.workSocket.Connected: {1}", _fileNm, state.workSocket.Connected);

                if (state.workSocket.Connected)
                {
                    state.deviceInfo.TrackerDataActionTime = DateTime.UtcNow;
                    state.deviceInfo.TrackerIp = state.workSocket.RemoteEndPoint.ToString();

                    // Read data from the client socket.
                    int bytesRead = state.workSocket.EndReceive(ar);
                    log.DebugFormat("{0}/ReadCallback: bytesRead: {1}", _fileNm, bytesRead);
                    if (bytesRead == 0)
                    {
                        state.Dispose();
                        return;
                    }

                    //TODO memory will increase in this case
                    state.deviceInfo.RawData = new byte[bytesRead];
                    state.deviceInfo.RawData = state.buffer.Take(bytesRead).ToArray();

                    state.deviceInfo = Protocol.Parser.Process(this.ProtocolParser, state.deviceInfo);

                    log.DebugFormat("{0}/ReadCallback: Protocol.Protocol.Process Done", _fileNm);

                    if (state.deviceInfo.ToSendRawData != null && state.deviceInfo.ToSendRawData.Count() > 0)
                    {
                        log.InfoFormat("{0}/ReadCallback: ToSendRawData", _fileNm);
                        // TODO sent to device
                        Send(state.workSocket, state.deviceInfo.ToSendRawData);
                        state.deviceInfo.ToSendRawData = null;
                    }

                    log.DebugFormat("{0}/ReadCallback: Payload: {1}, ParserStatus: {2}", _fileNm, state.deviceInfo.Payload, state.deviceInfo.ParserStatus);

                    if (state.deviceInfo.Payload != null && state.deviceInfo.Payload.Length > 0 &&
                        state.deviceInfo.ParserStatus == ProtocolParserStatus.Initialized)
                    {
                        log.DebugFormat("{0}/ReadCallback: Unparsed junk closing connection", _fileNm);
                        // Unparsed junk stored in Payload,
                        // Close the connection to avoid payload increase in its size
                        state.Dispose();
                    }
                    else
                    {
                        if (isListening)
                        {
                            state.workSocket.BeginReceive(state.buffer, 0, state.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                        }
                    }
                }
                else
                {
                    state.Dispose();
                }
            }
            catch (SocketException sException)
            {
                // Can be handled only by exception: Client disconnected
                log.ErrorFormat("{0}/ReadCallback: SocketException Client: {1}, Closed Exception: {2}", _fileNm, state.workSocket.RemoteEndPoint, sException);
                state.Dispose();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}/ReadCallback: {1}", _fileNm, ex);
            }
        }

        public void Send(Socket handler, byte[] byteData)
        {
            try
            {
                log.InfoFormat("{0}/Send:", _fileNm);
                log.DebugFormat("{0}/Send: Bytes length: {1} Bytes: {2}", _fileNm, byteData.Length, byteData);
                // Begin sending the data to the remote device.
                if (isListening)
                {
                    handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}/Send: {1}", _fileNm, ex);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                log.InfoFormat("{0}/SendCallback:", _fileNm);
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception e)
            {
                log.ErrorFormat("{0}/SendCallback: {1}", _fileNm, e);
            }
        }
        
        //private void DisconnectCallback(IAsyncResult ar)
        //{
        //    Console.WriteLine("DisconnectCallback..........");
        //    // Complete the disconnect request.
        //    Socket client = (Socket)ar.AsyncState;
        //    client.EndDisconnect(ar);
        //}
        
        public void Stop(bool forceStop)
        {
            log.InfoFormat("{0}/Stop: forceStop: {1}", _fileNm, forceStop);

            Console.WriteLine("Stopping server for: {0} with port: {1}", this.ProtocolName, this.Port);

            this.isListening = false;
            allDone.Set();

            Console.WriteLine(ConnectedClientObjs.Count + " Connected clients");

            for (int i = 0; i < ConnectedClientObjs.Count; i++)
            {
                if (ConnectedClientObjs[i]._workSocket.Connected)
                {
                    try
                    {
                        log.DebugFormat("{0}/Stop: Closing connection for EndPoint {1}", _fileNm, ConnectedClientObjs[i]._workSocket.RemoteEndPoint);

                        Console.WriteLine("{0}/Stop: Closing connection for EndPoint {1}", _fileNm, ConnectedClientObjs[i]._workSocket.RemoteEndPoint);
                        
                        ConnectedClientObjs[i].Dispose();
                        ConnectedClientObjs.Remove(ConnectedClientObjs[i]);
                        //ConnectedClientObjs[i]._workSocket.Shutdown(SocketShutdown.Both);
                        //ConnectedClientObjs[i]._workSocket.Close();
                        //ConnectedClients[i].BeginDisconnect(true, new AsyncCallback(DisconnectCallback), ConnectedClients[i]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);

                        log.ErrorFormat("{0}/Stop: Exception: {1}", _fileNm, ex);
                    }
                }
            }
        }
    }
}
