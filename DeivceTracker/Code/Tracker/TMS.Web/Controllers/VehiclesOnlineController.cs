using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using TMS.Web.Models.ViewModels;
using DAL;
using TMS.DAL;
using TMS.Web.Rules;
using TMS.Model;
using AutoMapper;
using TMS.BusinessRule.Interfaces;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
namespace TMS.Web.Controllers
{
    public class VehiclesOnlineController : Controller
    {
        public VehiclesOnlineController()
        {
        }

        public ActionResult Home()
        {
            return RedirectToAction("List");
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult TwoWheelers()
        {
            return RedirectToAction("List");
        }

        public ActionResult FourWheelers()
        {
            return RedirectToAction("List");
        }

        public ActionResult Tracking()
        {
            List<AvailableDeviceViewModel> devices = new List<AvailableDeviceViewModel>();

            User userObj = Session["UserData"] as User;

            devices = GetDeviceList(userObj.UserId.ToString());
            return View(devices);
        }

        public ActionResult MultiTracking()
        {
            List<AvailableDeviceViewModel> devices = new List<AvailableDeviceViewModel>();

            User userObj = Session["UserData"] as User;

            devices = GetDeviceList(userObj.UserId.ToString());
            return View(devices);
        }

        public ActionResult GetDeviceData(string DeviceId)
        {
            List<DeviceDataViewModel> devices = new List<DeviceDataViewModel>();
            return Json(devices, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public List<AvailableDeviceViewModel> GetDeviceList(string UserId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("UserId", UserId));

            DataTable dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure, "Api_GetDeviceList", parameters.ToArray());

            List<AvailableDeviceViewModel> devices = new List<AvailableDeviceViewModel>();
            if (dt != null && dt.Rows.Count > 0)
            {
                devices = dt.AsEnumerable().Select(r => new AvailableDeviceViewModel()
                {
                    DeviceId = (Convert.ToString(r["DeviceId"])),
                    IMEINo = (Convert.ToString(r["IMEINo"])),
                    VehicleNo = (Convert.ToString(r["VehicleNo"])),
                    Make = (Convert.ToString(r["Make"])),
                    VehicleModel = (Convert.ToString(r["VehicleModel"])),
                    VehicleType = (Convert.ToString(r["VehicleType"])),
                    SimNetwork = (Convert.ToString(r["SimNetwork"])),
                    DeviceSimNo = (Convert.ToString(r["DeviceSimNo"])),
                    TimeZone = (Convert.ToString(r["TimeZone"])),
                    EntryDate = (Convert.ToString(r["EntryDate"])),
                    ExpiryDate = (Convert.ToString(r["ExpiryDate"])),
                    DeviceType = (Convert.ToString(r["DeviceType"]))
                }).ToList();
            }

            Session["UserDevices"] = devices.Select(m => m.DeviceId).ToList();

            return devices;
        }

        [HttpGet]
        public ActionResult GetCurrentDevicesData(string DevicesList)
        {
            List<VehicleOnlineViewModel> dataList = new List<VehicleOnlineViewModel>();

            JavaScriptSerializer js = new JavaScriptSerializer();
            List<string> requestedDevices = js.Deserialize<List<string>>(DevicesList);

            List<string> UserDevices = Session["UserDevices"] as List<string>;

            if (UserDevices != null && UserDevices.Count > 0)
            {
                List<string> devicesToTake = new List<string>();

                devicesToTake = requestedDevices.Where(m => UserDevices.Contains(m)).ToList();
                //devicesToTake = devicesToTake.Take(50).ToList();

                DataTable dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure,
                    "Api_GetDeviceCurrentData", new SqlParameter[] {
                        new SqlParameter("DeviceList", string.Join(",", devicesToTake.ToArray()))
                    });

                if (dt != null && dt.Rows.Count > 0)
                {
                    dataList = dt.AsEnumerable().ToList().Select(dr => new VehicleOnlineViewModel()
                    {
                        DeviceId = Convert.ToString(dr["DeviceId"]),
                        Status = 200,
                        ErrorMessage = null,
                        Data = new VehicleOnlineViewModel.DeviceData()
                        {
                            Latitude = Convert.ToString(dr["Latitude"]),
                            Longitude = Convert.ToString(dr["Longitude"]),
                            Direction = Convert.ToString(dr["Direction"])
                        }
                    }).ToList();
                }
            }

            List<string> identifiedList = new List<string>();

            if (dataList.Count > 0)
            {
                identifiedList = dataList.Select(m => m.DeviceId).ToList();
            }

            List<string> unIdentifiedList = requestedDevices.Where(m1 => !identifiedList.Any(m2 => m1 == m2)).ToList();
            if (unIdentifiedList != null && unIdentifiedList.Count > 0)
            {
                dataList.AddRange(unIdentifiedList.Select(c => new VehicleOnlineViewModel()
                {
                    DeviceId = c,
                    Status = 403,
                    ErrorMessage = "Unauthorized Device",
                    Data = null
                }).ToList());
            }

            return Json(dataList, JsonRequestBehavior.AllowGet);
        }
    }
}