using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrackerManagementSystem.Web.Models.Display;

namespace TrackerManagementSystem.Web.Controllers
{
    public class DeviceController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetDeviceList()
        {
            DataTable dt = DAL.Data.GetData(DAL.DataBase.Api, System.Data.CommandType.Text,
                "select Distinct(DeviceId) from DeviceData where CommandType = 2 ");

            List<Device> devices = new List<Device>();
            if (dt != null && dt.Rows.Count > 0)
            {
                devices = dt.AsEnumerable().Select(r => new Device()
                {
                    deviceId = (Convert.ToString(r["DeviceId"])),
                    isOnline = true
                }).ToList();
            }

            return Json(devices, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCurrentPosition(string DeviceId)
        {

            DataTable dt = DAL.Data.GetData(DAL.DataBase.Api, System.Data.CommandType.Text, 
                "Select * FROM DeviceData WHERE CommandType = 2 AND DeviceId = '" + DeviceId + "'");

            var position = new Position();
            if (dt != null && dt.Rows.Count > 0)
            {
                position.Latitude = decimal.Parse(Convert.ToString(dt.Rows[0]["Latitude"]));
                position.Longitude = decimal.Parse(Convert.ToString(dt.Rows[0]["Longitude"]));
            }

            return Json(position, JsonRequestBehavior.AllowGet);
        }
    }
}