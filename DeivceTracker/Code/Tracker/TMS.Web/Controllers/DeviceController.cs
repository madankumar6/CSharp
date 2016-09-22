using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.BusinessRule.Interfaces;
using TMS.Model;
using TMS.Web.Models;
using TMS.Web.Models.ViewModels;
using TMS.Web.Rules;

namespace TMS.Web.Controllers
{
    public class DeviceController : Controller
    {
        #region MyRegion
        private readonly ICustomerService _customerService;
        private readonly IDistributorService _distributorService;
        private readonly IDealerService _dealerService;
        private readonly IVehicleService<Vehicle> _vehicleService;
        private readonly IDeviceService<Device> _deviceServce;
        #endregion
        public DeviceController(ICustomerService customerService, IDistributorService distributorService, IDealerService dealerService, IVehicleService<Vehicle> vehicleService, IDeviceService<Device> deviceService)
        {
            this._customerService = customerService;
            this._distributorService = distributorService;
            this._dealerService = dealerService;
            this._vehicleService = vehicleService;
            this._deviceServce = deviceService;
        }

        public ActionResult GetDevices(int draw, int start, int length, string distributorId)
        {
            var currentUser = Session["UserData"] as CustomPrincipalSerializeModel;

            if (Session["UserData"] is Distributor)
            {
                var distributor = Session["UserData"] as Distributor;
                distributorId = distributor.UserId.ToString();
            }
            if (distributorId != null)
            {

                var deviceGuid = Guid.Parse(distributorId);

                if (start < 1)
                    start = 1;
                else
                    start = (start / length) + 1;

                var _vehicle = _deviceServce.GetDevices(deviceGuid, start, length);
                var vehiclesData = Mapper.Map<List<Device>, List<DeviceViewModel>>(_vehicle.Items).Select(Vehicle => new { Vehicle.IMEINo, Vehicle.ModelNo, Vehicle.SIMNo, Vehicle.NetworkProvider });

                return Json(new
                {
                    draw = draw,
                    recordsTotal = _vehicle.TotalItems,
                    recordsFiltered = _vehicle.TotalItems,
                    data = vehiclesData
                }, JsonRequestBehavior.AllowGet);
            }
            return null;
        }
        // GET: Device
        public ActionResult Index()
        {
            if (Session["UserData"] is Admin)
            {
                var currentUser = Session["UserData"] as Admin;
                var distributors = GetDistributors(currentUser.UserId.ToString());
                var distributorsSelectList =
                    new SelectList(
                        distributors.Select(
                            dist => new SelectListItem() { Text = dist.Username, Value = dist.UserId.ToString() }), "Value",
                        "Text");

                ViewData["DistributorList"] = distributorsSelectList;

                var dealers = GetDealers(currentUser.UserId.ToString());
                var dealersSelectList =
                    new SelectList(
                        dealers.Select(
                            deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value",
                        "Text");

                ViewData["DealerList"] = dealersSelectList;
                var customers = GetCustomers(currentUser.UserId.ToString());
                var customersSelectList =
                    new SelectList(
                        customers.Select(
                            deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value",
                        "Text");

                ViewData["customerList"] = customersSelectList;

                var vehicles = GetVehicles(currentUser.UserId.ToString());
                var vehiclesSelectList =
                    new SelectList(
                        vehicles.Select(
                            deal => new SelectListItem() { Text = deal.VehicleNo, Value = deal.VehicleId.ToString() }), "Value",
                        "Text");

                ViewData["vehicleList"] = vehiclesSelectList;
            }
            return View();
        }

        private List<Distributor> GetDistributors(string adminId)
        {
            if (!string.IsNullOrWhiteSpace(adminId))
            {
                return _distributorService.GetDistributors(Guid.Parse(adminId));
            }
            return null;
        }

        private List<Dealer> GetDealers(string distributorId)
        {
            if (!string.IsNullOrWhiteSpace(distributorId))
            {
                return _dealerService.GetDealers(Guid.Parse(distributorId));
            }
            return null;
        }


        private List<Customer> GetCustomers(string dealerId)
        {
            if (!string.IsNullOrWhiteSpace(dealerId))
            {
                return _customerService.GetCustomers(Guid.Parse(dealerId));
            }
            return null;
        }

        private List<Vehicle> GetVehicles(string customerId)
        {
            if (!string.IsNullOrWhiteSpace(customerId))
            {
                return _vehicleService.GetVehicles(Guid.Parse(customerId));
            }
            return null;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetDealer(string dealerId)
        {
            var dealer = GetDealers(dealerId.ToString());
            var dealerLst = new SelectList(
                dealer.Select(
                    deal => new SelectListItem()
                    {
                        Text = deal.Username,
                        Value = deal.UserId.ToString()

                    }), "Value", "Text");
            ViewData["DealerList"] = dealerLst;

            return Json(dealerLst, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCustomer(string customerId)
        {
            var customer = GetCustomers(customerId.ToString());
            var customerList = new SelectList(
                customer.Select(
                    cust => new SelectListItem()
                    {
                        Text = cust.Username,
                        Value = cust.UserId.ToString()

                    }), "Value", "Text");
            ViewData["customerList"] = customerList;

            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetVehicle(string vehicleId)
        {
            var customer = GetVehicles(vehicleId.ToString());
            var customerList = new SelectList(
                customer.Select(
                    cust => new SelectListItem()
                    {
                        Text = cust.VehicleNo,
                        Value = cust.VehicleId.ToString()

                    }), "Value", "Text");
            ViewData["vehicleList"] = customerList;

            return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            if (Session["UserData"] is Admin)
            {
                var currentUser = Session["UserData"] as Admin;
                var distributors = GetDistributors(currentUser.UserId.ToString());
                var distributorsSelectList =
                    new SelectList(
                        distributors.Select(
                            dist => new SelectListItem() { Text = dist.Username, Value = dist.UserId.ToString() }), "Value",
                        "Text");

                ViewData["DistributorList"] = distributorsSelectList;
                var currentUsers = Session["UserData"] as Admin;
                var dealers = GetDealers(currentUsers.UserId.ToString());
                var dealerSelectList =
                    new SelectList(
                        dealers.Select(
                            deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value",
                        "Text");

                ViewData["dealerList"] = dealerSelectList;
                var customers = GetCustomers(currentUser.UserId.ToString());
                var customerSelectList =
                    new SelectList(
                        customers.Select(cust => new SelectListItem() { Text = cust.Username, Value = cust.UserId.ToString() }), "Value", "Text");
                ViewData["customerList"] = customerSelectList;
                var vehicles = GetVehicles(currentUser.UserId.ToString());
                var vehicleSelectList =
                    new SelectList(
                        vehicles.Select(cust => new SelectListItem() { Text = cust.VehicleNo, Value = cust.VehicleId.ToString() }), "Value", "Text");
                ViewData["vehicleList"] = vehicleSelectList;
            }

            //return View(new VehicleViewModel());
            DeviceViewModel device = new DeviceViewModel();
            // DeviceMapping devicemap = new DeviceMapping();
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Add", device);
            }
            return View(device);
        }

      //  private TMSEntities mapping = new TMSEntities();
        // POST : /Distributor/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DeviceViewModel module)
        {
            HttpPostedFileBase file = Request.Files["fileUpload"];
            if (ModelState.IsValid)
            {
                try
                {
                    var device = Mapper.Map<DeviceViewModel, Device>(module);
                    // var devicemapping = Mapper.Map<DeviceMap, Models.DeviceMapping>(modum);
                    if (Session["UserData"] is Distributor)
                    {
                        device.CustomerId = ((Distributor)Session["UserData"]).UserId;
                    }
                    else
                    {
                        device.CustomerId = Guid.Parse(Request.Form["customerId"]);
                        device.VehicleId = Guid.Parse(Request.Form["vehicleId"]);
                        device.ImageName = file.FileName;
                        device.Image = ConvertToBytes(file);
                    }

                    // Models.DeviceMapping devicemap = new Models.DeviceMapping();
                    // var devicemap = new DeviceMapping();

                    //devicemap.VehicleId = Guid.Parse(Request.Form["vehicleId"]);
                    //devicemap.DeviceId = Guid.Parse(module.DeviceId.ToString());

                    //mapping.deviceMap.Add(devicemap);
                    //mapping.SaveChanges();

                    _deviceServce.CreateDevice(device);
                    _deviceServce.SaveDevice();

                    return RedirectToAction("Index", "Device");
                }

                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("Error", ex.Message);
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("Error", ex.Message);
                }
            }

            if (Request.IsAjaxRequest())
                return PartialView("_Add", module);

            return View(module);
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
        // GET : /Distributor/Edit
        public ActionResult Edit(string deviceId)
        {
            DeviceViewModel deviceVM = new DeviceViewModel();
            try
            {
                if (Session["UserData"] is Admin)
                {
                    var currentUser = Session["UserData"] as Admin;
                    var distributors = GetDistributors(currentUser.UserId.ToString());
                    var distributorsSelectList =
                        new SelectList(
                            distributors.Select(
                                dist => new SelectListItem() { Text = dist.Username, Value = dist.UserId.ToString() }), "Value",
                            "Text", deviceVM.DeviceId.ToString());

                    ViewData["DistributorList"] = distributorsSelectList;

                    var dealers = GetDealers(currentUser.UserId.ToString());
                    var dealersSelectList = new SelectList(
                                                 dealers.Select(deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value", "Text");
                    ViewData["DealerList"] = dealersSelectList;
                    var customers = GetCustomers(currentUser.UserId.ToString());
                    var customersSelectList = new SelectList(
                                                 customers.Select(cust => new SelectListItem() { Text = cust.Username, Value = cust.UserId.ToString() }), "Value", "Text");
                    ViewData["customerList"] = customersSelectList;
                    var vehicles = GetVehicles(currentUser.UserId.ToString());
                    var vehicleSelectList =
                        new SelectList(
                            vehicles.Select(cust => new SelectListItem() { Text = cust.VehicleNo, Value = cust.VehicleId.ToString() }), "Value", "Text");
                    ViewData["vehicleList"] = vehicleSelectList;

                    if (string.IsNullOrWhiteSpace(deviceId))
                    {
                        deviceVM.DeviceId = Guid.Empty;
                    }
                    else
                    {
                        var deviceGuid = Guid.Parse(deviceId);
                        Device device = _deviceServce.GetDevice(deviceGuid);
                        deviceVM = Mapper.Map<Device, DeviceViewModel>(device);
                    }
                }
                    return PartialView("_Edit", deviceVM);
            }
            catch (Exception ex)
            {
                //     Logger.ErrorFormat("Controller : {0} - Action : {1}, Message : {2}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), ex.Message);
            }

            return View(deviceVM);
        }

        // POST : /Distributor/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DeviceViewModel module)
        {
            //model.Password = "temp";
            if (Session["UserData"] is Admin)
            {
                module.CustomerId = Guid.Parse(Request.Form["customerId"]);
            }
            HttpPostedFileBase file = Request.Files["fileUpload"];

            if (ModelState.IsValid)
            {
                try
                {
                    if (module.DeviceId == Guid.Empty)
                    {
                        module.DeviceId = Guid.NewGuid();
                        var device = Mapper.Map<DeviceViewModel, Device>(module);
                        // var devicemapping = Mapper.Map<DeviceMap, Models.DeviceMapping>(modum);                     
                        device.CustomerId = Guid.Parse(Request.Form["customerId"]);
                        device.VehicleId = Guid.Parse(Request.Form["vehicleId"]);
                        if (file != null)
                        {
                            device.ImageName = file.FileName;
                            device.Image = ConvertToBytes(file);
                        }
                        _deviceServce.CreateDevice(device);
                    }
                    else
                    {
                        var device = _deviceServce.GetDevice(module.DeviceId);
                        Mapper.Map<DeviceViewModel, Device>(module, device);
                        _deviceServce.UpdateDevice(device);
                    }
                    _deviceServce.SaveDevice();

                    return Json(new { Result = "Success" });
                }
                catch (Exception ex)
                {

                }
            }

            if (Session["UserData"] is Admin)
            {
                var currentUser = Session["UserData"] as Admin;
                var distributors = GetDistributors(currentUser.UserId.ToString());
                var distributorsSelectList =
                    new SelectList(
                        distributors.Select(
                            dist => new SelectListItem() { Text = dist.Username, Value = dist.UserId.ToString() }), "Value",
                        "Text", module.CustomerId.ToString());

                ViewData["DistributorList"] = distributorsSelectList;
                var dealers = GetDealers(currentUser.UserId.ToString());
                var dealersSelectList = new SelectList(
                                             dealers.Select(deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value", "Text");
                ViewData["DealerList"] = dealersSelectList;
                var customers = GetCustomers(currentUser.UserId.ToString());
                var customersSelectList = new SelectList(
                                             customers.Select(cust => new SelectListItem() { Text = cust.Username, Value = cust.UserId.ToString() }), "Value", "Text");
                ViewData["customerList"] = customersSelectList;
                var vehicles = GetVehicles(currentUser.UserId.ToString());
                var vehicleSelectList =
                    new SelectList(
                        vehicles.Select(cust => new SelectListItem() { Text = cust.VehicleNo, Value = cust.VehicleId.ToString() }), "Value", "Text");
                ViewData["vehicleList"] = vehicleSelectList;

            }
            return PartialView("_Edit", module);
        }

        // POST : /Distributor/Delete
        [HttpPost]
        public ActionResult Delete(List<string> devices)
        {
            try
            {
                _deviceServce.DeleteDevice(devices.ToArray());
                _deviceServce.SaveDevice();
                return Json(new { Result = "Success" });
            }
            catch (Exception)
            {
                return Json(new { Result = "Failure" });
            }
        }

    }
}