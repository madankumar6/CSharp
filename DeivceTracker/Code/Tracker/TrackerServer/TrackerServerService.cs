using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Utils;

namespace TrackerServer
{
    public partial class TrackerServer : ServiceBase
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

        //Tracker.TcpServer.Main tcpServer = null;
        Tracker.TcpServer.TcpServerSAEA.Main server = null;

        public TrackerServer()
        {
            InitializeComponent();
            //tcpServer = new Tracker.TcpServer.Main();
            server= new Tracker.TcpServer.TcpServerSAEA.Main();
        }

        protected override void OnStart(string[] args)
        {
            log.InfoFormat("{0}/OnStart", _fileNm);
            server.Start();
        }

        protected override void OnStop()
        {
            try
            {
                log.InfoFormat("OnStop Start", _fileNm);
                //tcpServer.Stop();
                server.Stop();
                log.InfoFormat("OnStop End", _fileNm);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("OnStop: {0}", ex);
            }
        }
    }
}
