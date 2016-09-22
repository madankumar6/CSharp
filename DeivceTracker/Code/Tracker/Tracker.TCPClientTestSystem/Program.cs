using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using Tracker.TcpClient.Config;

namespace Tracker.TCPClientTestSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClientTest.TcpClientTest client = new TcpClientTest.TcpClientTest();

            var tHandler = new Thread(() =>
            {
                client.Start();
            });
            tHandler.Start();

            Console.WriteLine("Press [q] to quit the client test");

            do
            {
            } while (Console.ReadKey().KeyChar != 'q');

            client.Stop();
            tHandler.Abort();
        }
    }
}
