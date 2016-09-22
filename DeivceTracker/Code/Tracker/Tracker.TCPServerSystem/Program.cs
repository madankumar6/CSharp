using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracker.TcpServer;

namespace Tracker.TCPServerSystem
{
    class Program
    {
        static Tracker.TcpServer.TcpServerSAEA.Main server = null;
        static void Main(string[] args)
        {
            //Tracker.TcpServer.Main tcpServer = new Main();
            //tcpServer.Start();

            ////Console.WriteLine("{0} Protocols are configured", serverTasks.Count);
            //Console.WriteLine("Press [q] to quit the server");

            //do
            //{
            //} while (Console.ReadKey().KeyChar != 'q');

            //tcpServer.Stop();

            //Console.WriteLine("Done");
            //Console.ReadLine();

            server = new TcpServer.TcpServerSAEA.Main();
            server.Start();
            Console.ReadLine();

            server.Stop();
        }
    }
}
