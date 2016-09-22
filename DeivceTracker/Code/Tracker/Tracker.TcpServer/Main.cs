using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracker.TcpServer.RequestHandlerV1;
using Utils;

namespace Tracker.TcpServer
{
    public class Main
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

        List<ITcpServer> serverManager;
        List<Task> serverTasks;

        CancellationTokenSource cancellationTokenSource;

        public Main()
        {
            serverManager = new List<ITcpServer>();
            serverTasks = new List<Task>();
            cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            log.InfoFormat("{0}/Start", _fileNm);

            string ProtocolsNPorts = ConfigurationManager.AppSettings["ProtocolsNPorts"];
            log.DebugFormat("{0}/Start: ProtocolsNPorts: {1}", _fileNm, ProtocolsNPorts);

            foreach (var pNPItem in ProtocolsNPorts.Split(','))
            {
                Console.WriteLine(pNPItem);
                var pNp = pNPItem.Split('|');
                serverTasks.Add(Task.Factory.StartNew(() => StartProtocolServer(pNp[0], pNp[1]), cancellationTokenSource.Token));

                //var tHandler = new Thread(() =>
                //{
                //    StartProtocolServer();
                //});

            }
        }

        public void Stop()
        {
            try
            {
                Console.WriteLine(serverManager.Count);

                foreach (var server in serverManager)
                {
                    server.Stop(true);
                }

                cancellationTokenSource.Cancel();
                Task.WaitAll(serverTasks.ToArray());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        private void StartProtocolServer(string ProtocolName, string ProtocolPort)
        {
            log.InfoFormat("{0}/StartProtocolServer", _fileNm);

            TcpServerV0_1 server = new TcpServerV0_1();
            serverManager.Add(server);
            server.Start(ProtocolName, IPAddress.Any, Convert.ToInt32(ProtocolPort));
        }
    }
}