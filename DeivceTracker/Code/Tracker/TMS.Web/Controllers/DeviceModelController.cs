using AutoMapper;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.BusinessRule.Interfaces;
using TMS.DAL;
using TMS.Model;
using TMS.Web.Models.ViewModels;
using TMS.Web.Rules;

namespace TMS.Web.Controllers
{
    public class DeviceModelController : Controller
    {

        private readonly IDeviceDataModelService devicemodelService;
        private readonly ICustomerService customerService;
        private readonly IDistributorService distributorService;
        private readonly IDealerService dealerService;
        private readonly IDeviceTypeService devicetypeService;

        public DeviceModelController(IDeviceDataModelService devicemodelService, ICustomerService customerService, IDistributorService distributorService, IDealerService dealerService, IDeviceTypeService devicetypeService)
        {
            this.devicemodelService = devicemodelService;
            this.customerService = customerService;
            this.distributorService = distributorService;
            this.dealerService = dealerService;
            this.devicetypeService = devicetypeService;
        }

        private List<Distributor> GetDistributors(string adminId)
        {
            if (!string.IsNullOrWhiteSpace(adminId))
            {
                return distributorService.GetDistributors(Guid.Parse(adminId));
            }
            return null;
        }

        private List<Dealer> GetDealers(string distributorId)
        {
            if (!string.IsNullOrWhiteSpace(distributorId))
            {
                return dealerService.GetDealers(Guid.Parse(distributorId));
            }
            return null;
        }

        public List<Customer> GetCustomers(string dealerId)
        {
            if (!string.IsNullOrWhiteSpace(dealerId))
            {
                return customerService.GetCustomers(Guid.Parse(dealerId));
            }
            return null;
        }

        public ActionResult GetVehicles(int draw, int start, int length, string distributorId)
        {
            var currentUser = Session["UserData"] as CustomPrincipalSerializeModel;

            if (Session["UserData"] is Distributor)
            {
                var distributor = Session["UserData"] as Distributor;
                distributorId = distributor.UserId.ToString();
            }

            //    if (distributorId != null && distributorId != "" && distributorId != 0.ToString())
            try
            {
                var VehicleGuid = Guid.Parse(distributorId);

                if (start < 1)
                    start = 1;
                else
                    start = (start / length) + 1;

                var _vehicle = devicemodelService.GetDevices(VehicleGuid, start, length);
                var vehiclesData = Mapper.Map<List<DeviceModels>, List<DeviceModelViewModel>>(_vehicle.Items).Select(Vehicle => new { Vehicle.DeviceId, Vehicle.VehicleNo, Vehicle.IMEINo, Vehicle.DeviceSimNo, Vehicle.Mail });

                return Json(new
                {
                    draw = draw,
                    recordsTotal = _vehicle.TotalItems,
                    recordsFiltered = _vehicle.TotalItems,
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


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetDealer(string dealerId)
        {
            if (dealerId != null && dealerId != 0.ToString())
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
            return null;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCustomer(string customerId)
        {
            if (customerId != null && customerId != 0.ToString())
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
            return null;
        }
        // GET: DeviceModel
        public ActionResult Index()
        {
            if (Session["UserData"] is Admin)
            {
                var currentUser = Session["UserData"] as Admin;
                var distributors = GetDistributors(currentUser.UserId.ToString());
                var distributorsSelectList = new SelectList(
                        distributors.Select(
                            dist => new SelectListItem() { Text = dist.Username, Value = dist.UserId.ToString() }), "Value",
                        "Text");

                ViewData["DistributorList"] = distributorsSelectList;
                List<Dealer> dealers = new List<Dealer>();
                SelectList dealersSelectList;

                if (distributors.FirstOrDefault() != null && distributors.Count != 0)
                {
                    dealers = GetDealers(distributors.First().UserId.ToString());
                    dealersSelectList =
                        new SelectList(
                            dealers.Select(
                                deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value",
                            "Text");
                }
                else
                {
                    dealersSelectList = new SelectList(new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = "0" } }, "Value", "Text");
                }

                ViewData["DealerList"] = dealersSelectList;
                SelectList customersSelectList;
                if (dealers.FirstOrDefault() != null)
                {
                    //  var currentUser = Session["UserData"] as Admin;
                    var customers = GetCustomers(dealers.First().UserId.ToString());
                    customersSelectList =
                     new SelectList(
                         customers.Select(
                             cust => new SelectListItem() { Text = cust.Username, Value = cust.UserId.ToString() }), "Value",
                         "Text");
                }
                else
                {
                    customersSelectList = new SelectList(new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = "0" } }, "Value", "Text");
                }
                ViewData["CustomerList"] = customersSelectList;
            }
            return View();
        }

        // GET : DeviceModel/Add/
        public ActionResult Add(string dealerId)
        {
            // TMSEntities db = new TMSEntities();
            //List<ProtocolServerViewModel> protocol = new List<ProtocolServerViewModel>();
            DataTable dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure, "Report_ProtocolServers");
            List<ProtocolServerViewModel> Server = new List<ProtocolServerViewModel>();
            if (dt != null || dt.Rows.Count > 0)
            {
                Server = dt.AsEnumerable().Select(m => new ProtocolServerViewModel()
                {
                    ProtocolServer = Convert.ToString(m["ProtocolServer"]),

                }).ToList();
            }
            DeviceModelViewModel dealerVM = new DeviceModelViewModel();
            if (Session["UserData"] is Admin)
            {
                var deviceTypes = Server.ToList();
                var deviceTypesSelectList =
                    new SelectList(
                        deviceTypes.Select(
                            dist => new SelectListItem() { Text = dist.ProtocolServer, Value = dist.ProtocolServer }), "Value",
                        "Text");

                ViewData["DeviceTypeList"] = deviceTypesSelectList;
                var currentUser = Session["UserData"] as Admin;
                var distributors = GetDistributors(currentUser.UserId.ToString());
                var distributorsSelectList =
                    new SelectList(
                        distributors.Select(
                            dist => new SelectListItem() { Text = dist.Username, Value = dist.UserId.ToString() }), "Value",
                        "Text");

                ViewData["DistributorList"] = distributorsSelectList;
                List<Dealer> dealers = new List<Dealer>();
                SelectList dealersSelectList;
                if (distributors.FirstOrDefault() != null)
                {
                    dealers = GetDealers(distributors.First().UserId.ToString());
                    dealersSelectList =
                         new SelectList(
                             dealers.Select(
                                 deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value",
                             "Text");
                }
                else
                {
                    dealersSelectList = new SelectList(new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = "0" } }, "Value", "Text");
                }

                ViewData["DealerList"] = dealersSelectList;
                SelectList customersSelectList;

                if (dealers.FirstOrDefault() != null)
                {
                    //  var currentUser = Session["UserData"] as Admin;
                    var customers = GetCustomers(dealers.First().UserId.ToString());
                    customersSelectList =
                     new SelectList(
                         customers.Select(
                             cust => new SelectListItem() { Text = cust.Username, Value = cust.UserId.ToString() }), "Value",
                         "Text");
                }
                else
                {
                    customersSelectList = new SelectList(new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = "0" } }, "Value", "Text");
                }

                ViewData["CustomerList"] = customersSelectList;

                if (string.IsNullOrWhiteSpace(dealerId))
                {
                    dealerVM.DeviceId = Guid.Empty;

                }
                else
                {
                    var dealerGuid = Guid.Parse(dealerId);
                    DeviceModels customer = devicemodelService.GetDevice(dealerGuid);
                    dealerVM = Mapper.Map<DeviceModels, DeviceModelViewModel>(customer);
                    dealerVM.ToExpiry = 0;
                }
            }

            return PartialView("_Add", dealerVM);
        }

        // POST : DeviceModel/Add/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DeviceModelViewModel devceModel)
        {

            if (Session["UserData"] is Admin)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (devceModel.DeviceId == Guid.Empty)
                        {
                            devceModel.DeviceId = Guid.NewGuid();
                            var deal = Mapper.Map<DeviceModelViewModel, DeviceModels>(devceModel);
                            //     deal.parent = Guid.Parse(Request.Form["CustomerId"]);
                            deal.UserId = Guid.Parse(Request.Form["DeviceUserId"]);
                            deal.ExpiryDate = DateTime.UtcNow.AddDays(deal.ToExpiry);
                            deal.TimeZone = Request.Form["DropDownTimezone"];
                            deal.VehicleType = Request.Form["VehicleType"];
                            devicemodelService.CreateDevice(deal);
                            devicemodelService.SaveDevice();
                        }
                        else
                        {
                            var customer = devicemodelService.GetDevice(devceModel.DeviceId);
                            Mapper.Map<DeviceModelViewModel, DeviceModels>(devceModel, customer);
                            customer.UserId = Guid.Parse(Request.Form["DeviceUserId"]);
                            customer.TimeZone = Request.Form["DropDownTimezone"];
                            customer.VehicleType = Request.Form["VehicleType"];
                            devicemodelService.UpdateDevice(customer);
                            devicemodelService.SaveDevice();
                        }

                        return Json(new { Result = "Success" });
                    }
                    catch (Exception ex)
                    {

                    }
                }
                DataTable dt = Data.GetData(DataBase.Api, CommandType.StoredProcedure, "Report_ProtocolServers");
                List<ProtocolServerViewModel> Server = new List<ProtocolServerViewModel>();
                if (dt != null || dt.Rows.Count > 0)
                {
                    Server = dt.AsEnumerable().Select(m => new ProtocolServerViewModel()
                    {
                        ProtocolServer = Convert.ToString(m["ProtocolServer"]),

                    }).ToList();
                }
                var deviceTypes = Server.ToList();
                var deviceTypesSelectList =
                    new SelectList(
                        deviceTypes.Select(
                            dist => new SelectListItem() { Text = dist.ProtocolServer, Value = dist.ProtocolServer }), "Value",
                        "Text");

                ViewData["DeviceTypeList"] = deviceTypesSelectList;
                var currentUser = Session["UserData"] as Admin;
                var distributors = GetDistributors(currentUser.UserId.ToString());
                var distributorsSelectList =
                    new SelectList(
                        distributors.Select(
                            dist => new SelectListItem() { Text = dist.Username, Value = dist.UserId.ToString() }), "Value",
                        "Text");

                ViewData["DistributorList"] = distributorsSelectList;
                var dealers = GetDealers(distributors.First().UserId.ToString());
                var dealersSelectList =
                    new SelectList(
                        dealers.Select(
                            deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value",
                        "Text");

                ViewData["DealerList"] = dealersSelectList;
                SelectList customersSelectList;
                if (dealers.FirstOrDefault() != null)
                {
                    //  var currentUser = Session["UserData"] as Admin;
                    var customers = GetCustomers(dealers.First().UserId.ToString());
                    customersSelectList =
                     new SelectList(
                         customers.Select(
                             cust => new SelectListItem() { Text = cust.Username, Value = cust.UserId.ToString() }), "Value",
                         "Text");
                }
                else
                {
                    customersSelectList = new SelectList(new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = "0" } }, "Value", "Text");
                }
                ViewData["CustomerList"] = customersSelectList;

            }
            return PartialView("_Add", devceModel);
        }


        // POST : /DeviceModel/Delete
        [HttpPost]
        public ActionResult Delete(List<string> devices)
        {
            try
            {
                devicemodelService.DeleteDevice(devices.ToArray());
                devicemodelService.SaveDevice();
                return Json(new { Result = "Success" });
            }
            catch (Exception)
            {
                return Json(new { Result = "Failure" });
            }
        }
    }
}