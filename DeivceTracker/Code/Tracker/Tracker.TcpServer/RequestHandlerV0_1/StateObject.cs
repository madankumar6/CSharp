using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using Tracker.Common.Model;
using Utils;

namespace Tracker.TcpServer.RequestHandlerV0_1
{
    // State object for reading client data asynchronously
    public class StateObject : IDisposable
    {
        #region LogProperties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

        // Client  socket.
        public Socket _workSocket = null;

        public Socket workSocket
        {
            get
            {
                return _workSocket;
            }
            set
            {
                _workSocket = value;
            }
        }

        // Size of receive buffer.
        public int BufferSize;
        // Receive buffer.
        public byte[] buffer;
        // Received data string.
        public StringBuilder sb = new StringBuilder();

        public DeviceInfo deviceInfo { get; set; }

        public DateTime connectedTime { get; set; }
        public DateTime lastReadTime { get; set; }
        public bool workSocketForcelyClosed { get; set; }
        public Timer connectionCheckTimer { get; set; }

        public StateObject()
        {
            BufferSize = Convert.ToInt32(ConfigurationManager.AppSettings["ConnectionReadBufferSize"] ?? "1024");
            buffer = new byte[BufferSize];

            connectedTime = DateTime.UtcNow;
            lastReadTime = connectedTime;

            // TODO close long connection which is waiting for data
            //http://www.codeproject.com/Articles/5733/A-TCP-IP-Server-written-in-C
            // CheckClientCommInterval

            //connectionCheckTimer = new Timer(5000);
            //connectionCheckTimer.Elapsed += connectionCheckTimer_Elapsed;
            //connectionCheckTimer.Start();
        }

        void connectionCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Console.WriteLine("From timer: EndPoint {0}", workSocket.RemoteEndPoint);
                connectionCheckTimer.Stop();

                Console.WriteLine("Connection idle time reached, Closing connection");

                workSocket.Shutdown(SocketShutdown.Both);
                workSocket.Disconnect(true);
                workSocket.Close(0);
                workSocketForcelyClosed = true;
            }
            catch (Exception ex)
            {
            }
        }

        public void Dispose()
        {
            log.InfoFormat("{0}/Dispose", _fileNm);
            try
            {
                this.BufferSize = 0;
                this.buffer = null;
                this.sb = null;
                if (this.workSocket != null && this.workSocket.Connected == true)
                {
                    log.DebugFormat("{0}/Dispose: Closing connection for EndPoint {1}", _fileNm, workSocket.RemoteEndPoint);

                    this.workSocket.Shutdown(SocketShutdown.Both);
                    this.workSocket.Close(0);
                    this.workSocket = null;
                }
                this.deviceInfo = null;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Dispose: {0}", ex);
            }
        }
    }
}
