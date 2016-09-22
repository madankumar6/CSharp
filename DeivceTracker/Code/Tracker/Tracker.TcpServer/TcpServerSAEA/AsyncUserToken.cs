using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Tracker.TcpServer.RequestHandlerV1
{
    class AsyncUserToken
    {
        private Socket _socket;

        public AsyncUserToken()
        {
        }

        // public property to get and set the internal private _socket member
        public Socket Socket
        {
            get { return _socket; }
            set { _socket = value; }
        }
    }
}
