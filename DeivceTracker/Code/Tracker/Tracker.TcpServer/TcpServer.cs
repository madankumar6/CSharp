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
    public class TcpServer
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);

        TcpListener server = null;
        #endregion

        public void StartServer()
        {
            Console.WriteLine("TCPServer Started...");
            server = new TcpListener(IPAddress.Any, Convert.ToInt32(ConfigurationManager.AppSettings["TCPPort"]));
            server.Start();
            while (true)
            {
                log.DebugFormat("Waiting for a connection...");
                //Console.Write("Waiting for a connection...-- ");
                TcpClient client = server.AcceptTcpClient();
                log.DebugFormat("New client connected...");
                //Console.WriteLine("new client connected");
                ThreadPool.QueueUserWorkItem(new WaitCallback(HandleClient), client);
            }
        }

        private void HandleClient(object tcpClient)
        {
            try
            {
            TcpClient client = (TcpClient)tcpClient;
            Byte[] bytes = new Byte[256];
            String data = null;
            int i;

            NetworkStream stream = client.GetStream();
            while (client.Connected && (i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                log.DebugFormat("From: {0}, Data: {1}", client.Client.RemoteEndPoint, data);
                //Console.WriteLine(data);
            }

            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}", ex);
            }
        }
    }
}