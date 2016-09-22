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
    public class ReportIgnitionController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return RedirectToAction("ProtocolServer");
           // return View();
        }

        public ActionResult ProtocolServer()
        {
            List<ReportIgnitionViewModel> Server = new List<ReportIgnitionViewModel>();
            Server = ProtocolServers();
            return View( );
        }


        public List<ReportIgnitionViewModel> ProtocolServers()
        {
            // List<ProtocolServerViewModel> reportModel = new List<ProtocolServerViewModel>();
             DataTable dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure, "Report_Ignition");
            List<ReportIgnitionViewModel> Server = new List<ReportIgnitionViewModel>();
            if (dt != null || dt.Rows.Count > 0)
            {
                Server = dt.AsEnumerable().Select(m => new ReportIgnitionViewModel()
                {
                    RN = Convert.ToInt32(m["RN"]),
                    OnAcc = Convert.ToInt32(m["OnAcc"]),
                    StartDate = Convert.ToDateTime(m["StartDate"]),
                    StopDate = Convert.ToDateTime(m["StopDate"]),
                    Duration = Convert.ToInt32(m["Duration"]),
                    StartLatitude = Convert.ToString(m["StartLatitude"]),
                    StartLongitude=Convert.ToString(m["StartLongitude"]),
                    StopLatitude=Convert.ToString(m["StopLatitude"]),
                    StopLongitude = Convert.ToString(m["StopLongitude"])

                }).ToList();            

            }
            return Server;
             // reportModel = new ProtocolServerModels().ProtocolServers( );
    

        }






    }
}