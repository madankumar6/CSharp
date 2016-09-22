using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Utils;

namespace Tracker.TcpServer.TcpServerSAEA
{
    public class Main
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

        SocketListener sl = null;

        public void Start()
        {
            try
            {
                Int32 numConnections = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfConnections"] ?? "1");
                Int32 bufferSize = Convert.ToInt32(ConfigurationManager.AppSettings["ConnectionBufferSize"] ?? "50");
                string ProtocolsNPorts = ConfigurationManager.AppSettings["ProtocolsNPorts"];

                List<string[]> items = ProtocolsNPorts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Split(new[] { '|' })).ToList();

                var protocolNPort = items.Select(s =>
                    new Tracker.TcpServer.TcpServerSAEA.Model.ProtocolNPort()
                    {
                        Key = s[0],
                        Value = s[1]
                    }).ToList();

                if (numConnections >= protocolNPort.Count)
                {
                    sl = new SocketListener(numConnections, bufferSize);
                    sl.Start(protocolNPort);
                    log.DebugFormat("{0}/Start: Server listening. Press any key to terminate the server process...", _fileNm);
                }
                else
                {
                    log.DebugFormat("{0}/Start: Server failed to start. NoOfConnections should be higher than Protocol count", _fileNm);
                }

            }
            catch (IndexOutOfRangeException iore)
            {
                log.ErrorFormat("{0}/Start: {1}", _fileNm, iore);
            }
            catch (FormatException fe)
            {
                log.ErrorFormat("{0}/Start: {1}", _fileNm, fe);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}/Start: {1}", _fileNm, ex);
            }
        }


        public void Stop()
        {
            try
            {
                log.InfoFormat("Stop: {0}", "Started");
                sl.Stop();
                log.InfoFormat("Stop: {0}", "Finished");
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Stop: {0}", ex);
            }
        }
    }
}
