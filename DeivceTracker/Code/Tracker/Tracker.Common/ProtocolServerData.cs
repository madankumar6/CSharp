using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Tracker.Common.Model;
using Utils;
using _DAL = DAL;

namespace Tracker.Common
{
    public abstract class ProtocolServerData
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

        public static bool SaveData(string ProtocolServer, int Port, string Action, string ActionText)
        {
            try
            {
                log.DebugFormat("{0}/SaveData:", _fileNm);

                var dbParams = new SqlParameter[]{
                    new SqlParameter("@ProtocolServer", ProtocolServer),
                    new SqlParameter("@Port", Port),
                    new SqlParameter("@Action", Action),
                    new SqlParameter("@ActionText", ActionText)
                };

                _DAL.Data.StoreData_ExecuteNonQuery(_DAL.DataBase.TcpServer, System.Data.CommandType.StoredProcedure, "T_StoreProtocolServer", dbParams);

                log.InfoFormat("{0}/SaveData: Finished", _fileNm);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}/SaveData: {1}", _fileNm, ex);
            }
            return true;
        }        
    }
}
