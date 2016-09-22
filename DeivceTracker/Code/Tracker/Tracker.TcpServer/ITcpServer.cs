using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Utils;

namespace Tracker.TcpServer
{
    public interface ITcpServer
    {
        void Start(string protocol, IPAddress iPAddress, int port);
        void Stop(bool forceStop);
    }
}