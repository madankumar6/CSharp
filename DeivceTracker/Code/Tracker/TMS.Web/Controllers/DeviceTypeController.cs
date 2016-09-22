using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.BusinessRule.Interfaces;
using TMS.Model;
using TMS.Web.Models.ViewModels;
using TMS.Web.Rules;

namespace TMS.Web.Controllers
{
    public class DeviceTypeController : Controller
    {
        private readonly IDeviceTypeService devicetypeService;

        public DeviceTypeController(IDeviceTypeService devicetypeService)
        {
            this.devicetypeService = devicetypeService;
        }

        public ActionResult GetDeviceTypes(int draw, int start, int length)
        {
            var currentUser = Session["UserData"] as CustomPrincipalSerializeModel;

            //    if (distributorId != null && distributorId != "" && distributorId != 0.ToString())
            try
            {
                if (start < 1)
                    start = 1;
                else
                    start = (start / length) + 1;

                var devicetype = devicetypeService.GetDeviceTypes(start, length);
                var vehiclesData = Mapper.Map<List<DeviceType>, List<DeviceTypeViewModel>>(devicetype.Items).Select(Vehicle => new { Vehicle.DeviceTypeId, Vehicle.DeviceModel_Type });

                return Json(new
                {
                    draw = draw,
                    recordsTotal = devicetype.TotalItems,
                    recordsFiltered = devicetype.TotalItems,
                    data = vehiclesData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }
            return Json(new
            {
                draw = draw,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = new DeviceModelViewModel()
            }, JsonRequestBehavior.AllowGet);
        }



        // GET: DeviceType
        public ActionResult Index()
        {
            return View();
        }

        // GET : DeviceType/Add
        public ActionResult Add(string deviceId)
        {
            DeviceTypeViewModel deviceVM = new DeviceTypeViewModel();
            if(string.IsNullOrWhiteSpace(deviceId))
            {
                deviceVM.DeviceTypeId = Guid.Empty;
            }
            else
            {
                var dealerGuid = Guid.Parse(deviceId);
                DeviceType deviceType = devicetypeService.GetDeviceType(dealerGuid);
                deviceVM = Mapper.Map<DeviceType, DeviceTypeViewModel>(deviceType);
            }
            return PartialView("_Add", deviceVM);
        }
        
        //POST : DeviceType/Add
        [HttpPost]
        public ActionResult Add(DeviceTypeViewModel  deviceType)
        {

            if (Session["UserData"] is Admin)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (deviceType.DeviceTypeId == Guid.Empty)
                        {
                            deviceType.DeviceTypeId = Guid.NewGuid();
                            var device = Mapper.Map<DeviceTypeViewModel, DeviceType>(deviceType);
                            //     deal.parent = Guid.Parse(Request.Form["CustomerId"]);
                            devicetypeService.CreateDeviceType(device);
                            devicetypeService.SaveDeviceType();

                        }
                        else
                        {
                            var device = devicetypeService.GetDeviceType(deviceType.DeviceTypeId);
                            Mapper.Map<DeviceTypeViewModel, DeviceType>(deviceType, device);
                            devicetypeService.UpdateDeviceType(device);
                            devicetypeService.SaveDeviceType();

                        }

                        return Json(new { Result = "Success" });
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return PartialView("_Add", deviceType);
        }

        // POST : /DeviceModel/Delete
        [HttpPost]
        public ActionResult Delete(List<string> devices)
        {
            try
            {
                devicetypeService.DeleteDeviceType(devices.ToArray());
                devicetypeService.SaveDeviceType();
                return Json(new { Result = "Success" });
            }
            catch (Exception)
            {
                return Json(new { Result = "Failure" });
            }
        }
    }
}