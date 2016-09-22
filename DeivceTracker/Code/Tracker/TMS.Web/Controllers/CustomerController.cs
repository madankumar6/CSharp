using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using TMS.BusinessRule.Interfaces;
using TMS.Model;
using TMS.Web.Models.ViewModels;
using TMS.Web.Rules;
using Utils;
using TMS.Web.Controllers;

namespace TMS.Web.Controllers
{
    public class CustomerController : Controller
    {
        #region Properties
        private static readonly string FileName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType?.ToString();
        internal static Log4NetWrap Logger = new Log4NetWrap(FileName);

        private readonly ICustomerService _customerService;
        private readonly IDistributorService _distributorService;
        private readonly IDealerService _dealerService;
        #endregion

        public CustomerController(ICustomerService customerService, IDistributorService distributorService, IDealerService dealerService)
        {
            this._customerService = customerService;
            this._distributorService = distributorService;
            this._dealerService = dealerService;
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

        public ActionResult GetCustomers(int draw, int start, int length, string distributorId)
        {
            var currentUser = Session["UserData"] as CustomPrincipalSerializeModel;

            if (Session["UserData"] is Distributor)
            {
                var distributor = Session["UserData"] as Distributor;
                distributorId = distributor.UserId.ToString();
            }

            try
            {
                var dealerGuid = Guid.Parse(distributorId);

                if (start < 1)
                    start = 1;
                else 
                    start = (start / length) + 1;

                var customers = _customerService.GetCustomers(dealerGuid, start, length);
                var customersData = Mapper.Map<List<Customer>, List<CustomerViewModel>>(customers.Items).Select(customer => new { customer.UserId, customer.FirstName, customer.LastName, customer.Username, customer.PhoneNo, customer.Email });

                return Json(new
                {
                    draw = draw,
                    recordsTotal = customers.TotalItems,
                    recordsFiltered = customers.TotalItems,
                    data = customersData
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
                data = new CustomerViewModel()
            }, JsonRequestBehavior.AllowGet);
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
        // GET: Distributor
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
                SelectList dealerSelectList;
                if (distributors != null && distributors.Count > 0)
                {
                    var dealers = GetDealers(distributors.First().UserId.ToString());
                    dealerSelectList =
                        new SelectList(
                            dealers.Select(
                                deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value",
                            "Text");
                }
                else
                {
                    dealerSelectList = new SelectList(new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = "0" } }, "Value", "Text");
                }
                    ViewData["dealerList"] = dealerSelectList;
            }
            return View();
        }
        // GET : /Distributor/Add
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

                SelectList dealerSelectList;
                if (distributors.First() != null)
                {
                    // var currentUsers = Session["UserData"] as Distributor;
                    var dealers = GetDealers(distributors.First().UserId.ToString());
                    dealerSelectList =
                        new SelectList(
                            dealers.Select(
                                deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value",
                            "Text");
                }
                else {
                    dealerSelectList = new SelectList(new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = "0" } }, "Value", "Text");
                }
                ViewData["dealerList"] = dealerSelectList;

            }

            //return View(new DistributorViewModel());
            CustomerViewModel dealer = new CustomerViewModel();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Add", dealer);
            }
            return View(dealer);
        }


        // POST : /Distributor/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var deal = Mapper.Map<CustomerViewModel, TMS.Model.Customer>(model);

                    if (Session["UserData"] is Distributor)
                    {
                        deal.Parent = ((Distributor)Session["UserData"]).UserId;
                    }
                    else
                    {
                        deal.Parent = Guid.Parse(Request.Form["dealerId"]);
                    }

                    _customerService.CreateCustomer(deal);
                    _customerService.SaveCustomer();

                    return RedirectToAction("Index", "Customer");
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
                return PartialView("_Add", model);

            return View(model);
        }
        // GET : /Distributor/Edit
        public ActionResult Edit(string customerId)
        {
            CustomerViewModel customerVM = new CustomerViewModel();
            if (Session["UserData"] is Admin)
            {
                var currentUser = Session["UserData"] as Admin;
                var distributors = GetDistributors(currentUser.UserId.ToString());
                var distributorsSelectList =
                    new SelectList(
                        distributors.Select(
                            dist => new SelectListItem() { Text = dist.Username, Value = dist.UserId.ToString() }), "Value",
                        "Text", customerVM.Parent.ToString());

                ViewData["DistributorList"] = distributorsSelectList;
                SelectList dealersSelectList;
                if (distributors.First() != null)
                {

                    var dealers = GetDealers(distributors.First().UserId.ToString());
                    dealersSelectList =
                       new SelectList(
                           dealers.Select(
                               deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Value",
                           "Text", customerVM.Parent.ToString());
                }
                else
                {
                    dealersSelectList = new SelectList(new List<SelectListItem>() { new SelectListItem() { Text = "Select", Value = "0" } }, "Value", "Text");
                }
                ViewData["DealerList"] = dealersSelectList;
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    customerVM.UserId = Guid.Empty;
                }
                else
                {
                    var customerGuid = Guid.Parse(customerId);
                    Customer customer = _customerService.GetUser(customerGuid);
                    customerVM = Mapper.Map<Customer, CustomerViewModel>(customer);
                }
            }
            return PartialView("_Edit", customerVM);
        }

        // POST : /Distributor/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerViewModel model)
        {
            model.Password = "temp";
            if (Session["UserData"] is Admin)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (model.UserId == Guid.Empty)
                        {
                            model.UserId = Guid.NewGuid();
                            var deal = Mapper.Map<CustomerViewModel, TMS.Model.Customer>(model);
                            deal.Parent = Guid.Parse(Request.Form["dealerId"]);
                            _customerService.CreateCustomer(deal);
                        }
                        else
                        {
                            var customer = _customerService.GetUser(model.UserId);

                            Mapper.Map<CustomerViewModel, Customer>(model, customer);
                            _customerService.UpdateUser(customer);
                        }
                        _customerService.SaveCustomer();

                        return Json(new { Result = "Success" });
                    }
                    catch (Exception ex)
                    {

                    }
                }
                var currentUser = Session["UserData"] as Admin;
                var distributors = GetDistributors(currentUser.UserId.ToString());
                var distributorsSelectList =
                    new SelectList(
                        distributors.Select(
                            dist => new SelectListItem() { Text = dist.Username, Value = dist.UserId.ToString() }), "Value",
                        "Text", model.Parent.ToString());

                ViewData["DistributorList"] = distributorsSelectList;
                var dealers = GetDealers(distributorsSelectList.SelectedValue.ToString());
                var dealersSelectList = new SelectList
                (dealers.Select(deal => new SelectListItem() { Text = deal.Username, Value = deal.UserId.ToString() }), "Text", "Value", model.UserId.ToString());
                ViewData["DealerList"] = dealersSelectList;

            }

            return PartialView("_Edit", model);
        }

        // POST : /Distributor/Delete
        [HttpPost]
        public ActionResult Delete(List<string> customers)
        {
            try
            {
                _customerService.DeleteCustomer(customers.ToArray());
                _customerService.SaveCustomer();
                return Json(new { Result = "Success" });
            }
            catch (Exception)
            {
                return Json(new { Result = "Failure" });
            }
        }
    }
}