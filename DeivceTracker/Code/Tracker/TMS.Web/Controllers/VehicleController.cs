using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.BusinessRule.Interfaces;
using TMS.Model;
using TMS.Web.Models.ViewModels;
using TMS.Web.Rules;
using Utils;

namespace TMS.Web.Controllers
{
    public class VehicleController : Controller
    {

        #region Properties
        private static readonly string FileName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType?.ToString();
        internal static Log4NetWrap Logger = new Log4NetWrap(FileName);
        #endregion

        #region MyRegion
        private readonly ICustomerService _customerService;
        private readonly IDistributorService _distributorService;
        private readonly IDealerService _dealerService;
        private readonly IVehicleService<Vehicle> _vehicleService;
        #endregion
        public VehicleController(ICustomerService customerService, IDistributorService distributorService, IDealerService dealerService, IVehicleService<Vehicle> vehicleService)
        {
            this._customerService = customerService;
            this._distributorService = distributorService;
            this._dealerService = dealerService;
            this._vehicleService = vehicleService;
        }
        public ActionResult GetVehicles(int draw, int start, int length, string distributorId)
        {
            var currentUser = Session["UserData"] as CustomPrincipalSerializeModel;

            if (Session["UserData"] is Distributor)
            {
                var distributor = Session["UserData"] as Distributor;
                distributorId = distributor.UserId.ToString();
            }

            if (distributorId != null)
            {
                var VehicleGuid = Guid.Parse(distributorId);

                if (start < 1)
                    start = 1;
                else
                    start = (start / length) + 1;

                var _vehicle = _vehicleService.GetVehicles(VehicleGuid, start, length);
                var vehiclesData = Mapper.Map<List<Vehicle>, List<VehicleViewModel>>(_vehicle.Items).Select(Vehicle => new { Vehicle.VehicleId, Vehicle.VehicleNo, Vehicle.Make, Vehicle.Type, Vehicle.Model });

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

        // GET: Vehicle
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

                ViewData["CustomerList"] = customersSelectList;
            }
            return View();
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
            var customerLst = new SelectList(
                customer.Select(
                    deal => new SelectListItem()
                    {
                        Text = deal.Username,
                        Value = deal.UserId.ToString()

                    }), "Value", "Text");
            ViewData["CustomerList"] = customerLst;

            return Json(customerLst, JsonRequestBehavior.AllowGet);
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

        public List<Customer> GetCustomers(string dealerId)
        {
            if (!string.IsNullOrWhiteSpace(dealerId))
            {
                return _customerService.GetCustomers(Guid.Parse(dealerId));
            }
            return null;
        }

        // GET : /Vehicle/Add
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

                var customers = GetCustomers(currentUsers.UserId.ToString());
                var customerSelectList =
                    new SelectList(
                        dealers.Select(
                            deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value",
                        "Text");

                ViewData["customerList"] = dealerSelectList;


            }

            //return View(new VehicleViewModel());
            VehicleViewModel vehicle = new VehicleViewModel();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Add", vehicle);
            }
            return View(vehicle);
        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }


        // POST : /Distributor/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(VehicleViewModel module)
        {

            HttpPostedFileBase file = Request.Files["fileUpload"];
            if (ModelState.IsValid)
            {
                try
                {
                    var vehicle = Mapper.Map<VehicleViewModel, Vehicle>(module);
                    if (Session["UserData"] is Distributor)
                    {
                        vehicle.CustomerId = ((Distributor)Session["UserData"]).UserId;
                    }
                    else
                    {
                        vehicle.CustomerId = Guid.Parse(Request.Form["customerId"]);
                        vehicle.ImageName = file.FileName;
                        vehicle.Image = ConvertToBytes(file);
                    }
                    _vehicleService.CreateVehicle(vehicle);
                    _vehicleService.SaveVehicle();

                    return RedirectToAction("Index", "Vehicle");
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
        // GET : /Distributor/Edit
        public ActionResult Edit(string vehicleId)
        {
            VehicleViewModel customerVM = new VehicleViewModel();
            if (Session["UserData"] is Admin)
            {
                if (string.IsNullOrWhiteSpace(vehicleId))
                {
                    customerVM.VehicleId = Guid.Empty;
                }
                else
                {
                    var vehicleGuid = Guid.Parse(vehicleId);
                    Vehicle vehicle = _vehicleService.GetVehicle(vehicleGuid);
                    customerVM = Mapper.Map<Vehicle, VehicleViewModel>(vehicle);
                }
                var currentUser = Session["UserData"] as Admin;
                var distributors = GetDistributors(currentUser.UserId.ToString());
                var distributorsSelectList =
                    new SelectList(
                        distributors.Select(
                            dist => new SelectListItem() { Text = dist.Username, Value = dist.UserId.ToString() }), "Value",
                        "Text");

                ViewData["DistributorList"] = distributorsSelectList;

                var dealers = GetDealers(currentUser.UserId.ToString());
                var dealersSelectList = new SelectList(
                                             dealers.Select(deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value", "Text");
                ViewData["DealerList"] = dealersSelectList;

                var customers = GetCustomers(currentUser.UserId.ToString());
                var customersSelectList = new SelectList(
                                             customers.Select(cust => new SelectListItem() { Text = cust.Username, Value = cust.UserId.ToString() }), "Value", "Text");
                ViewData["CustomerList"] = customersSelectList;
            }
            return PartialView("_Edit", customerVM);
        }


        // POST : /Distributor/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VehicleViewModel module)
        {
                HttpPostedFileBase file = Request.Files["fileUpload"];
            //model.Password = "temp";
            if (Session["UserData"] is Admin)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (module.VehicleId == Guid.Empty)
                        {

                            module.VehicleId = Guid.NewGuid();
                            var vehicle = Mapper.Map<VehicleViewModel, Vehicle>(module);
                            vehicle.CustomerId = Guid.Parse(Request.Form["custId"]);
                            if (file != null)
                            {
                                vehicle.ImageName = file.FileName;
                                vehicle.Image = ConvertToBytes(file);
                            }
                            _vehicleService.CreateVehicle(vehicle);
                            _vehicleService.SaveVehicle();

                        }
                        else
                        {
                            var vehicle = _vehicleService.GetVehicle(module.VehicleId);
                            if (file != null)
                            {
                                vehicle.ImageName = file.FileName;
                                vehicle.Image = ConvertToBytes(file);
                            }
                            Mapper.Map<VehicleViewModel, Vehicle>(module, vehicle);
                            _vehicleService.UpdateVehicle(vehicle);
                            _vehicleService.SaveVehicle();
                        }

                        return Json(new { Result = "Success" });
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorFormat("Controller : {0} - Action : {1}, Message : {2}", this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), ex.Message);

                    }
                }

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
                ViewData["CustomerList"] = customersSelectList;

            }
            if (Request.IsAjaxRequest())
                return PartialView("_Edit", module);
            return View(module);


        }
        // POST : /Distributor/Delete
        [HttpPost]
        public ActionResult Delete(List<string> vehicles)
        {
            try
            {
                _vehicleService.DeleteVehicle(vehicles.ToArray());
                _vehicleService.SaveVehicle();
                return Json(new { Result = "Success" });
            }
            catch (Exception)
            {
                return Json(new { Result = "Failure" });
            }
        }

        // POST : /Distributor/Delete
        //public ActionResult Index()
        //{
        //    TMSEntities dbContext = new TMSEntities();
        //    List<string> devices = new List<string>();
        //    // List<DeviceData> deviceInfos = new List<DeviceData>();
        //    // IEnumerable<string> result =  DeviceData.GetAllDevices().Select(x => x.DeviceId);
        //    //foreach (var item in result)
        //    // {
        //    //     devices.Add(item.ToString());
        //    // }
        //    IEnumerable<VehicleViewModel> empDetails = dbContext.Database.SqlQuery<VehicleViewModel>("select * from vehicle").ToList();
        //    if (empDetails != null)
        //    {
        //        IEnumerable<string> result = empDetails.AsEnumerable().Select(x => Convert.ToString(x.VehicleId)).Distinct().ToList();
        //        foreach (var item in result)
        //        {
        //            devices.Add(item.ToString());
        //        }
        //    }
        //    return View(devices);
        //}
    }
}