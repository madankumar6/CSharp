using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TMS.Web.Models.ViewModels;
//using Tracker.Common;
using Tracker.Common.Model;
using System.Linq;
using _DAL = DAL;
using TMS.DAL;
using Tracker.Common;
using System.Web.Mvc;

namespace TMS.Web.Models
{
    public class ProtocolServerModels
    {
        public List<ProtocolServerViewModel> ProtocolServers()
        {
            List<ProtocolServerViewModel> rData = new List<ProtocolServerViewModel>();

            try
            {

                DataTable dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure, "Report_ProtocolServer");

                if (dt != null || dt.Rows.Count > 0)
                {
                    rData = dt.AsEnumerable().Select(m => new ProtocolServerViewModel()
                    {
                        ProtocolServer = Convert.ToString(m["ProtocolServer"]),
                        Port = Convert.ToInt32(m["Port"]),
                        Action = Convert.ToString(m["Action"]),
                        DevicesConnected = Convert.ToInt32(m["DevicesConnected]"]),
                        ActionText = Convert.ToString(m["ActionText"]),
                        ActionTime = Convert.ToDateTime(m["ActionTime"]),
            
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return rData;

        }
      
    }
}