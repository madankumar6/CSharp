using System;
using System.Net.Sockets;

namespace Tracker.TcpServer.TcpServerSAEA.Model
{
    public class StateObject : IDisposable
    {
        public string ProtocolName { get; set; }
        public int Port { get; set; }

        public Tracker.Parser.Protocol _protocolParser { get; set; }
        public Tracker.Parser.Protocol ProtocolParser
        {
            get
            {
                if (_protocolParser == null)
                {
                    //Type.GetType("Tracker.Protocol.***Protocol, Tracker.Protocol");
                    Type t = Type.GetType("Tracker.Parser." + this.ProtocolName + "Protocol, Tracker.Protocol");
                    if (t != null)
                    {
                        _protocolParser = (Tracker.Parser.Protocol)Activator.CreateInstance(t);
                    }
                    else
                    {
                        _protocolParser = new Tracker.Parser.UnknownProtocol();
                    }
                }
                return _protocolParser;
            }
        }

        private Socket _socket;
        private byte[] rawData;

        // public property to get and set the internal private _socket member
        public Socket Connection
        {
            get { return _socket; }
            set { _socket = value; }
        }

        public string DeviceId { get; internal set; }

        public StateObject()
        {
        }

        public StateObject(Socket sock, Int32 bufferSize)
        {
            //this.sb = new StringBuilder(bufferSize);
            rawData = new byte[bufferSize];
            this.Connection = sock;
        }

        public void Dispose()
        {
            //this.deviceInfo = DeviceData.Dispose(this.deviceInfo);
            //this.deviceInfo = null;
        }
    }
}
