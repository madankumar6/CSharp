using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Tracker.Common;
using Tracker.Common.Model;
using Tracker.TcpServer.TcpServerSAEA.Model;
using Utils;

namespace Tracker.TcpServer.TcpServerSAEA
{
    /// <summary>
    /// Based on example from http://msdn2.microsoft.com/en-us/library/system.net.sockets.socketasynceventargs.aspx
    /// Implements the connection logic for the socket server.  
    /// After accepting a connection, all data read from the client is sent back. 
    /// The read and echo back to the client pattern is continued until the client disconnects.
    /// </summary>
    internal sealed class SocketListener
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

        #region ServerProperties

        /// <summary>
        /// The sockets used to listen for incoming connection requests for different ports.
        /// </summary>
        private List<Socket> listenSockets;

        /// <summary>
        /// Mutex to synchronize server execution.
        /// </summary>
        private static Mutex mutex = new Mutex();

        /// <summary>
        /// Buffer size to use for each socket I/O operation.
        /// </summary>
        private Int32 bufferSize;

        /// <summary>
        /// The total number of clients connected to the server.
        /// </summary>
        private Int32 numConnectedSockets;

        /// <summary>
        /// the maximum number of connections the sample is designed to handle simultaneously.
        /// </summary>
        private Int32 numConnections;

        /// <summary>
        /// Pool of reusable SocketAsyncEventArgs objects for write, read and accept socket operations.
        /// </summary>
        private SocketAsyncEventArgsPool readWritePool;

        /// <summary>
        /// Controls the total number of clients connected to the server.
        /// </summary>
        private Semaphore semaphoreAcceptedClients;
        #endregion

        #region Server Start to Listen

        /// <summary>
        /// Create an uninitialized server instance.  
        /// To start the server listening for connection requests
        /// call the Init method followed by Start method.
        /// </summary>
        /// <param name="numConnections">Maximum number of connections to be handled simultaneously.</param>
        /// <param name="bufferSize">Buffer size to use for each socket I/O operation.</param>
        internal SocketListener(Int32 numConnections, Int32 bufferSize)
        {
            log.InfoFormat("{0}/SocketListener", _fileNm);

            // TODO
            // Manually handle client disconnect, donot want to wait connections in OS pool
            // numConnections = numConnections + 2;

            this.numConnectedSockets = 0;
            this.numConnections = numConnections;
            this.bufferSize = bufferSize;

            this.readWritePool = new SocketAsyncEventArgsPool(numConnections);
            this.semaphoreAcceptedClients = new Semaphore(numConnections, numConnections);

            // Preallocate pool of SocketAsyncEventArgs objects.
            for (Int32 i = 0; i < this.numConnections; i++)
            {
                SocketAsyncEventArgs readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);
                readWriteEventArg.SetBuffer(new Byte[this.bufferSize], 0, this.bufferSize);

                // Add SocketAsyncEventArg to the pool.
                this.readWritePool.Push(readWriteEventArg);
            }
            log.InfoFormat("{0}/SocketListener Finished", _fileNm);
        }

        /// <summary>
        /// Starts the server listening for incoming connection requests.
        /// </summary>
        /// <param name="port">Port where the server will listen for connection requests.</param>
        //internal void Start(Int32 port)
        internal void Start(List<Tracker.TcpServer.TcpServerSAEA.Model.ProtocolNPort> ProtocolNPort, int numOfConnInPool = 0)
        {
            try
            {
                log.InfoFormat("{0}/Start", _fileNm);

                //// Get host related information.
                //IPAddress[] addressList = Dns.GetHostEntry(Environment.MachineName).AddressList;
                listenSockets = new List<Socket>();
                foreach (var pNP in ProtocolNPort)
                {
                    try
                    {
                        int port = Convert.ToInt32(pNP.Value);
                        // Create the socket which listens for incoming connections.
                        var _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        _listenSocket.ReceiveBufferSize = this.bufferSize;
                        _listenSocket.SendBufferSize = this.bufferSize;
                        // TODO implementation for IPv6
                        //if (localEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
                        //{
                        //    // Set dual-mode (IPv4 & IPv6) for the socket listener.
                        //    // 27 is equivalent to IPV6_V6ONLY socket option in the winsock snippet below,
                        //    // based on http://blogs.msdn.com/wndp/archive/2006/10/24/creating-ip-agnostic-applications-part-2-dual-mode-sockets.aspx
                        //    this.listenSocket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
                        //    this.listenSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, localEndPoint.Port));
                        //}

                        _listenSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                        // Start the server.
                        _listenSocket.Listen(this.numConnections);

                        listenSockets.Add(_listenSocket);

                        log.DebugFormat("{0}/Start at Port: {1} is for Protocol: {2}", _fileNm, pNP.Value, pNP.Key);

                        ProtocolServerData.SaveData(pNP.Key, port, "STARTED", string.Format("Started for: {0}", pNP.Key));

                        // Post accepts on the listening socket.
                        this.StartAccept(_listenSocket, null, pNP.Key, port);

                    }
                    catch (Exception innerEx)
                    {
                        ProtocolServerData.SaveData(pNP.Key, Convert.ToInt32(pNP.Value), "Error", string.Format("{0}", innerEx));
                        log.ErrorFormat("{0}/Start InnerException {1}", _fileNm, innerEx);
                        Console.WriteLine("{0}/Start InnerException {1}", _fileNm, innerEx);
                    }
                }
                // Blocks the current thread to receive incoming messages.
                mutex.WaitOne();

            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}/Start {1}", _fileNm, ex);
                Console.WriteLine("{0}/Start {1}", _fileNm, ex);
            }
        }

        /// <summary>
        /// Begins an operation to accept a connection request from the client.
        /// </summary>
        /// <param name="acceptEventArg">The context object to use when issuing 
        /// the accept operation on the server's listening socket.</param>
        private void StartAccept(Socket listenSocket, SocketAsyncEventArgs acceptEventArg, string ProtocolName, int Port)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs() { UserToken = new StateObject() { ProtocolName = ProtocolName, Port = Port } };
                acceptEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            }
            else
            {
                // Socket must be cleared since the context object is being reused.
                acceptEventArg.AcceptSocket = null;
            }
            // TODO: will release the thread for already assigned socket.
            // If another connection waits on another socket it gain access
            // Will be a problem on connections reaching maximum
            this.semaphoreAcceptedClients.WaitOne();
            if (!listenSocket.AcceptAsync(acceptEventArg))
            {
                this.ProcessAccept(listenSocket, acceptEventArg);
            }
        }

        /// <summary>
        /// Callback method associated with Socket.AcceptAsync 
        /// operations and is invoked when an accept operation is complete.
        /// </summary>
        /// <param name="sender">Object who raised the event.</param>
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>
        private void OnAcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            this.ProcessAccept(sender as Socket, e);
        }

        /// <summary>
        /// Process the accept for the socket listener.
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed accept operation.</param>
        private void ProcessAccept(Socket listenSocket, SocketAsyncEventArgs e)
        {
            // e is contains main socket
            Socket s = e.AcceptSocket;
            StateObject so = e.UserToken as StateObject;

            if (s.Connected)
            {
                try
                {
                    if (this.readWritePool.Count > 0)
                    {
                        SocketAsyncEventArgs readEventArgs = this.readWritePool.Pop();
                        if (readEventArgs != null)
                        {
                            // Get the socket for the accepted client connection and put it into the 
                            // ReadEventArg object user token.
                            readEventArgs.UserToken = new StateObject(s, this.bufferSize)
                            {
                                ProtocolName = so.ProtocolName,
                                Port = so.Port
                            };

                            Interlocked.Increment(ref this.numConnectedSockets);

                            Console.WriteLine("Client connected for {0}, {1}. There are {2} clients connected to the server", so.ProtocolName, so.Port, this.numConnectedSockets);
                            log.DebugFormat("{3}/ProcessAccept Client connected for {0}, {1}. There are {2} clients connected to the server", so.ProtocolName, so.Port, this.numConnectedSockets, _fileNm);

                            // Update to DB
                            DeviceData.UpdateNumOfDevicesConnected(Protocol: so.ProtocolName, Port: so.Port, Count: this.numConnectedSockets);

                            if (!s.ReceiveAsync(readEventArgs))
                            {
                                this.ProcessReceive(readEventArgs);
                            }
                        }
                        else
                        {
                            Console.WriteLine("There are no more available sockets to allocate.");
                        }
                    }
                }
                catch (SocketException ex)
                {
                    StateObject token = e.UserToken as StateObject;
                    log.ErrorFormat("Error when processing data received from {0}:\r\n{1}", token.Connection.RemoteEndPoint, ex.ToString());
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("{0}/ProcessAccept: ", _fileNm, ex);
                }

                // Accept the next connection request.
                this.StartAccept(listenSocket, e, so.ProtocolName, so.Port);
            }
            else
            {
                log.DebugFormat("ProcessAccept: >>>> Try 1 for connection close state");
            }
        }
        #endregion


        #region Read and Write
        /// <summary>
        /// Callback called whenever a receive or send operation is completed on a socket.
        /// </summary>
        /// <param name="sender">Object who raised the event.</param>
        /// <param name="e">SocketAsyncEventArg associated with the completed send/receive operation.</param>
        private void OnIOCompleted(object sender, SocketAsyncEventArgs e)
        {
            // Determine which type of operation just completed and call the associated handler.
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    this.ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    this.ProcessSend(e);
                    break;
                case SocketAsyncOperation.Disconnect:
                    log.DebugFormat("ProcessAccept: >>>> Try 2 for connection close state");
                    break;
                    //default:
                    //    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        /// <summary>
        /// This method is invoked when an asynchronous receive operation completes. 
        /// If the remote host closed the connection, then the socket is closed.  
        /// If data was received then the data is echoed back to the client.
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed receive operation.</param>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            try
            {
                log.DebugFormat("{0}/ProcessReceive received started...", _fileNm);

                // Check if the remote host closed the connection.
                if (e.BytesTransferred > 0)
                {
                    if (e.SocketError == SocketError.Success)
                    {
                        StateObject state = e.UserToken as StateObject;
                        if (state == null)// Already closed
                        {
                            return;
                        }
                        #region Process Protocol

                        string DeviceId = state.DeviceId;
                        DeviceInfo di = DeviceData.GetDeviceById(DeviceId);

                        di.TrackerDataActionTime = DateTime.UtcNow;
                        try
                        {
                            di.TrackerIp = state.Connection.RemoteEndPoint.ToString();
                        }
                        catch (Exception ex)
                        { }

                        //TODO memory will increase in this case
                        di.RawData = new byte[e.BytesTransferred];
                        di.RawData = e.Buffer.Take(e.BytesTransferred).ToArray();

                        log.DebugFormat("{0}/ProcessReceive received for Parse data: {1}", _fileNm, BitConverter.ToString(di.RawData));
                        di = Protocol.Parser.Process(state.ProtocolParser, di);
                        log.DebugFormat("{0}/ProcessReceive to send to client: {1}", _fileNm, di.ToSendRawData);

                        if (di.Payload != null && di.Payload.Length > 0 &&
                            di.ParserStatus == ProtocolParserStatus.Initialized)
                        {
                            // Unparsed junk stored in Payload,
                            // Close the connection to avoid payload increase in its size
                            this.CloseClientSocket(e);
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(DeviceId))
                        {
                            //Close existing connection
                            DeviceData.GetExistingConnectionsByDeviceId(di.DeviceId).ForEach(eCon =>
                            {
                                Console.WriteLine("Closing >>>>>>>>>>>>>>> {0}", di.DeviceId);
                                CloseClientSocket(eCon);
                            });

                            //DeviceData.RemoveConnectionsByDeviceId(di.DeviceId); handled in CloseClientSocket

                            // Add new connection
                            DeviceData.AddConnection(di.DeviceId, e);
                        }

                        state.DeviceId = di.DeviceId;
                        DeviceData.UpdateDevice(di);

                        if (di.ToSendRawData != null && di.ToSendRawData.Length > 0)
                        {
                            //if (state.Connection.Available == 0)
                            {
                                // TODO sent to device
                                log.DebugFormat("{0}/ProcessReceive {1}", _fileNm,
                                    BitConverter.ToString(di.ToSendRawData));
                                e.SetBuffer(di.ToSendRawData, 0, di.ToSendRawData.Length);
                                try
                                {
                                    // Set return buffer.
                                    if (!state.Connection.SendAsync(e))
                                    {
                                        // Set the buffer to send back to the client.
                                        this.ProcessSend(e);
                                    }

                                }
                                catch (Exception)
                                {
                                }
                            }
                            di.ToSendRawData = new byte[0];
                        }
                        else
                        {
                            log.DebugFormat("{0}/ProcessReceive received Bind receive for next data...", _fileNm);
                            try
                            {
                                if (!state.Connection.ReceiveAsync(e))
                                {
                                    // Read the next block of data sent by client.
                                    this.ProcessReceive(e);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                        #endregion
                    }
                    else
                    {
                        this.ProcessError(e);
                    }
                }
                else
                {
                    log.DebugFormat("ProcessAccept: >>>> Try 3 for connection close state");
                    // Closed by client
                    this.CloseClientSocket(e);
                }

            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// This method is invoked when an asynchronous send operation completes.  
        /// The method issues another receive on the socket to read any additional 
        /// data sent from the client.
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed send operation.</param>
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            log.DebugFormat("{0}/ProcessSend Start", _fileNm);
            if (e.SocketError == SocketError.Success)
            {
                e.SetBuffer(new Byte[this.bufferSize], 0, this.bufferSize);

                log.DebugFormat("{0}/ProcessSend SocketError: {1}", _fileNm, e.SocketError);
                log.DebugFormat("{0}/ProcessReceive received Bind receive for next data...", _fileNm);
                // Will be called as needed, not all time on sending data
                StateObject token = e.UserToken as StateObject;
                try
                {
                    if (!token.Connection.ReceiveAsync(e))
                    {
                        // Read the next block of data send from the client.
                        this.ProcessReceive(e);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                this.ProcessError(e);
            }
            log.DebugFormat("{0}/ProcessSend Finished", _fileNm);
        }

        #endregion

        /// <summary>
        /// Close the socket associated with the client.
        /// </summary>
        /// <param name="e">SocketAsyncEventArg associated with the completed send/receive operation.</param>
        public void CloseClientSocket(SocketAsyncEventArgs e)
        {
            StateObject token = e.UserToken as StateObject;
            if (token != null)
            {
                try
                {
                    int RemovedConnections = DeviceData.RemoveConnectionsByDeviceId(token.DeviceId);
                    Console.WriteLine("RemovedConnections: {0}", RemovedConnections);

                    DeviceData.UpdateDeviceOffline(token.DeviceId);

                    this.CloseClientSocket(token.Connection);
                    token.Dispose();
                    e.UserToken = null;// token;

                    Interlocked.Decrement(ref this.numConnectedSockets);
                    Console.WriteLine("A client has been disconnected from the server. There are {0} clients connected to the server", this.numConnectedSockets);
                    log.DebugFormat("A client has been disconnected from the server. There are {0} clients connected to the server", this.numConnectedSockets);
                    // Update to DB
                    DeviceData.UpdateNumOfDevicesConnected(Protocol: token.ProtocolName, Port: token.Port, Count: this.numConnectedSockets);

                    e.SetBuffer(new Byte[this.bufferSize], 0, this.bufferSize);
                    // Free the SocketAsyncEventArg so they can be reused by another client.
                    this.readWritePool.Push(e);

                    // Decrement the counter keeping track of the total number of clients connected to the server.
                    this.semaphoreAcceptedClients.Release();

                }
                catch (Exception ex)
                {
                }
            }
        }

        public void CloseClientSocket(Socket s)
        {
            try
            {
                s.Shutdown(SocketShutdown.Send);
            }
            catch (Exception ex)
            {
                // Throw if client has closed, so it is not necessary to catch.
                Console.WriteLine("Closing connection, Error: {0}", ex);
            }
            finally
            {
                s.Close();
            }
        }

        private void ProcessError(SocketAsyncEventArgs e)
        {
            try
            {

                StateObject token = e.UserToken as StateObject;
                IPEndPoint localEp = token.Connection.LocalEndPoint as IPEndPoint;

                this.CloseClientSocket(e);

                Console.WriteLine("Socket error {0} on endpoint {1} during {2}.", (Int32)e.SocketError, localEp, e.LastOperation);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Stop the server.
        /// </summary>
        internal void Stop()
        {
            for (int i = 0; i < listenSockets.Count; i++)
            {
                listenSockets[i].Close();
                //ProtocolServerData.SaveData(pNP.Key, listenSockets[i]., "STARTED", string.Format("Started for: {0}", pNP.Key));
            }
            mutex.ReleaseMutex();
        }
    }
}