using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Model;
using TMS.Web.Models;
using TMS.Web.Models.ViewModels;

namespace TMS.Web.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return RedirectToAction("SpeedReport");
        }

        public ActionResult Summary()
        {
            ReportViewModel reportModel = new ReportViewModel();
            reportModel.Parameter = new ReportParameterViewModel();
            return View(reportModel);
        }

        [HttpPost]
        public ActionResult Summary(ReportViewModel reportModel)
        {
            //ReportViewModel reportModel = new ReportViewModel();

            reportModel.Results = new List<ReportResultViewModel>();

            reportModel = new ReportData().GetSummaryReport(reportModel);
            return View(reportModel);
        }


        public ActionResult SpeedReport(string DeviceId)
        {
            ReportParameterViewModel model = new ReportParameterViewModel();
            model.DeviceList = new List<SelectListItem>();
            var CurrentUser = Session["UserData"] as Admin;
            // Get list of devices
            DataTable dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure, "Api_GetDeviceList",
                new List<SqlParameter>() {
                    new SqlParameter("UserId", Guid.Parse(CurrentUser.UserId.ToString())) }.ToArray());
            if (dt != null || dt.Rows.Count > 0)
            {
                model.DeviceList = dt.AsEnumerable().Select(m => new SelectListItem()
                {
                    Value = Convert.ToString(m["DeviceId"]),
                    Text = Convert.ToString(m["DeviceId"])
                }).ToList();
            }

            ReportViewModel reportModel = new ReportViewModel();

            reportModel.Parameter = new ReportParameterViewModel();
            reportModel.Parameter=model;
            return View(reportModel);
        }

        [HttpPost]
        public ActionResult SpeedReport(ReportViewModel reportModel)
        {
            reportModel.Results = new List<ReportResultViewModel>();
            reportModel = new ReportData().GetSpeedReport(reportModel);

            ReportParameterViewModel model = new ReportParameterViewModel();
            model.DeviceList = new List<SelectListItem>();
            var CurrentUser = Session["UserData"] as Admin;

            // Get list of devices
            DataTable dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure, "Api_GetDeviceList", new List<SqlParameter>() {
                    new SqlParameter("UserId", Guid.Parse(CurrentUser.UserId.ToString())) }.ToArray());
            if (dt != null || dt.Rows.Count > 0)
            {
                model.DeviceList = dt.AsEnumerable().Select(m => new SelectListItem()
                {
                    Value = Convert.ToString(m["DeviceId"]),
                    Text = Convert.ToString(m["DeviceId"])
                }).ToList();
            }
            reportModel.Parameter = model;
            return View(reportModel);
        }
    }
}