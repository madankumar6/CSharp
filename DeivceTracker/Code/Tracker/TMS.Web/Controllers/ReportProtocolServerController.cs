using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Web.Models;
using TMS.Web.Models.ViewModels;

namespace TMS.Web.Controllers
{
    public class ReportProtocolServerController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return RedirectToAction("ProtocolServer");
           // return View();
        }

        public ActionResult ProtocolServer()
        {
            List<ProtocolServerViewModel> Server = new List<ProtocolServerViewModel>();
            Server = ProtocolServers();
            return View(Server);
        }



        public List<ProtocolServerViewModel > ProtocolServers()
        {
            // List<ProtocolServerViewModel> reportModel = new List<ProtocolServerViewModel>();
             DataTable dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure, "Report_ProtocolServer");
            List<ProtocolServerViewModel> Server = new List<ProtocolServerViewModel>();
            if (dt != null || dt.Rows.Count > 0)
            {
                Server = dt.AsEnumerable().Select(m => new ProtocolServerViewModel()
                {
                    ProtocolServer = Convert.ToString(m["ProtocolServer"]),
                    Port = Convert.ToInt32(m["Port"]),
                    DevicesConnected = Convert.ToInt32(m["DevicesConnected"]),
                    Action = Convert.ToString(m["Action"]),
                    ActionText = Convert.ToString(m["ActionText"]),
                    ActionTime = Convert.ToDateTime(m["ActionTime"]),

                }).ToList();            

            }
            return Server;
             // reportModel = new ProtocolServerModels().ProtocolServers( );
    

        }






    }
}