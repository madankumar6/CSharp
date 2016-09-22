using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Web.Models;
using TMS.Web.Models.ViewModels;

namespace TMS.Web.Controllers
{
    public class SettingsController : Controller
    {

        public ActionResult DeviceSettings(string DeviceId)
        {
            DeviceSettingsViewModel model = new DeviceSettingsViewModel();
            model.DeviceList = new List<SelectListItem>();

            // Get list of devices
            DataTable dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure, "Api_GetDeviceList");
            if (dt != null || dt.Rows.Count > 0)
            {
                model.DeviceList = dt.AsEnumerable().Select(m => new SelectListItem()
                {
                    Value = Convert.ToString(m["DeviceId"]),
                    Text = Convert.ToString(m["DeviceId"])
                }).ToList();
            }

            //ViewData["DeviceId"] = model.DeviceList;

            if (string.IsNullOrWhiteSpace(DeviceId) && model.DeviceList.Count > 0)
            {
                DeviceId = model.DeviceList.First().Value;
            }

            // Get device configuration
            dt = null;
            dt = new DataTable();

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("DeviceId", DeviceId));

            dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure, "Master_GetDeviceSettings", parameters.ToArray());

            if (dt != null || dt.Rows.Count > 0)
            {
                var tModel = dt.AsEnumerable().Select(m => new DeviceSettingsViewModel()
                {
                    DeviceId = Convert.ToString(m["IMEINo"]),
                    Odometer = Convert.ToInt32(m["Odometer"]),
                }).FirstOrDefault();
                if (tModel != null)
                {
                    model.DeviceId = tModel.DeviceId;
                    model.Odometer = tModel.Odometer;
                }
            }

            model = model ?? new DeviceSettingsViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult DeviceSettings(DeviceSettingsViewModel model)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("DeviceId", model.DeviceId));
            parameters.Add(new SqlParameter("Odometer", model.Odometer));

            try
            {
                Data.StoreData_ExecuteNonQuery(DataBase.Api, CommandType.StoredProcedure, "Master_SaveDeviceSettings", parameters.ToArray());
                TempData["Result"] = true;
            }
            catch (Exception ex)
            {
                TempData["Result"] = false;
            }
            //return DeviceSettings(model.DeviceId);
            return RedirectToAction("DeviceSettings", new { DeviceId = model.DeviceId });
        }






        // GET: Odometer
        public ActionResult Index()
        {
            DataTable dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure, "ReportOdometer");
            List<DeviceSettingsViewModel> Server = new List<DeviceSettingsViewModel>();
            if (dt != null || dt.Rows.Count > 0)
            {
                Server = dt.AsEnumerable().Select(m => new DeviceSettingsViewModel()
                {
                    DeviceId = Convert.ToString(m["DeviceId"])

                    //IMEI = Convert.ToString(m["IMEI"]),
                    //Odometer = Convert.ToInt32(m["Odometer"]),
                    //ActionTime = Convert.ToDateTime(m["ActionTime"]),

                }).ToList();
                var dealerSelectList =
                   new SelectList(
                       Server.Select(
                           deal => new SelectListItem() { Text = deal.DeviceId, Value = deal.DeviceId.ToString() }), "Value",
                       "Text");
                ViewData["DeviceList"] = dealerSelectList;
            }
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult Device(string dealerId)
        {
            DataTable dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure, "ReportOdometer");
            List<DeviceSettingsViewModel> Server = new List<DeviceSettingsViewModel>();
            if (dt != null || dt.Rows.Count > 0)
            {
                Server = dt.AsEnumerable().Select(m => new DeviceSettingsViewModel()
                {
                    DeviceId = Convert.ToString(m["DeviceId"]),
                    Odometer = Convert.ToInt32(m["Odometer"])

                }).Where(m => m.DeviceId == dealerId).ToList();

            }
            ViewData["DealerList"] = Server;

            return Json(Server, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Save(DeviceDataViewModel deviceCalc)
        {
            //DataTable dt = Data.StoreData_ExecuteScalar(DataBase.Api, CommandType.TableDirect, "DeviceCalcData", new List() {
            //    new SqlParameter("Odometer",deviceCalc.Odometer) }.ToArray());
            //////List<DeviceCalcDataViewModel> Server = new List<DeviceCalcDataViewModel>();
            //if (dt != null || dt.Rows.Count > 0) 
            //{
            //    Server = dt.AsEnumerable().Select(m => new DeviceCalcDataViewModel()
            //    {
            //        DeviceId = Convert.ToString(m["DeviceId"]),
            //        Odometer = Convert.ToInt32(m["Odometer"])

            //    }).Where(m => m.DeviceId == dealerId).ToList();
            //}
            return View();
        }
    }

}