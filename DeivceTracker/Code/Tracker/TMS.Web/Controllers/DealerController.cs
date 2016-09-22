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
using System.IO;

namespace TMS.Web.Controllers
{
    public class DealerController : Controller
    {
        #region Properties
        private static readonly string FileName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType?.ToString();
        internal static Log4NetWrap Logger = new Log4NetWrap(FileName);

        private readonly IDealerService _dealerService;
        private readonly IDistributorService _distributorService;
        #endregion

        public DealerController(IDealerService dealerService, IDistributorService distributorService)
        {
            this._dealerService = dealerService;
            this._distributorService = distributorService;
        }

        private List<Distributor> GetDistributors(string adminId)
        {
            if (!string.IsNullOrWhiteSpace(adminId))
            {
                return _distributorService.GetDistributors(Guid.Parse(adminId));
            }
            return null;
        }

        public ActionResult GetDealers(int draw, int start, int length, string distributorId)
        {
            var currentUser = Session["UserData"] as CustomPrincipalSerializeModel;

            if (Session["UserData"] is Distributor)
            {
                var distributor = Session["UserData"] as Distributor;
                distributorId = distributor.UserId.ToString();
            }
            try
            {
                var distributorGuid = Guid.Parse(distributorId);

                if (start < 1)
                    start = 1;
                else
                    start = (start / length) + 1;

                var dealers = _dealerService.GetDealers(distributorGuid, start, length);
                var dealersData = Mapper.Map<List<Dealer>, List<DealerViewModel>>(dealers.Items).Select(dealer => new { dealer.UserId, dealer.FirstName, dealer.LastName, dealer.Username, dealer.PhoneNo, dealer.Email });

                return Json(new
                {
                    draw = draw,
                    recordsTotal = dealers.TotalItems,
                    recordsFiltered = dealers.TotalItems,
                    data = dealersData
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {

            }
            return Json(new
            {
                draw = draw,
                recordsTotal = 0,
                recordsFiltered = 0,
                data = new DealerViewModel()
            }, JsonRequestBehavior.AllowGet);
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
            }
            return View();
        }

        // POST : /Distributor/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DealerViewModel model)
        {
            HttpPostedFileBase file = Request.Files["fileUpload"];

            if (ModelState.IsValid)
            {
                try
                {
                    var dealer = Mapper.Map<DealerViewModel, Dealer>(model);

                    if (Session["UserData"] is Distributor)
                    {
                        dealer.Parent = ((Distributor)Session["UserData"]).UserId;
                    }
                    else
                    {
                        dealer.Parent = Guid.Parse(Request.Form["distributorId"]);
                        dealer.ImageName = file.FileName;
                        dealer.Logo = ConvertToBytes(file);

                    }

                    _dealerService.CreateDealer(dealer);
                    _dealerService.SaveDealer();

                    return RedirectToAction("Index", "Dealer");
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

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        // GET : /Distributor/Edit
        public ActionResult Edit(string dealerId)
        {
            DealerViewModel dealerVM = new DealerViewModel();
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

                if (string.IsNullOrWhiteSpace(dealerId))
                {
                    dealerVM.UserId = Guid.Empty;
                }
                else
                {
                    var dealerGuid = Guid.Parse(dealerId);
                    Dealer dealer = _dealerService.GetUser(dealerGuid);
                    dealerVM = Mapper.Map<Dealer, DealerViewModel>(dealer);
                }
            }
            return PartialView("_Edit", dealerVM);
        }


        // POST : /Distributor/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DealerViewModel model)
        {
                HttpPostedFileBase file = Request.Files["fileUpload"];
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
                            var dealer = Mapper.Map<DealerViewModel, Dealer>(model);

                            if (file != null)
                            {
                                dealer.ImageName = file.FileName;
                                dealer.Logo = ConvertToBytes(file);
                            }

                            _dealerService.CreateDealer(dealer);
                            _dealerService.SaveDealer();

                        }
                        else
                        {
                            var dealer = _dealerService.GetUser(model.UserId);

                            Mapper.Map<DealerViewModel, Dealer>(model, dealer);

                            if (file != null)
                            {
                                dealer.ImageName = file.FileName;
                                dealer.Logo = ConvertToBytes(file);
                            }

                            _dealerService.UpdateUser(dealer);
                            _dealerService.SaveDealer();
                        }
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
            }
            return PartialView("_Edit", model);
        }

        // POST : /Distributor/Delete
        [HttpPost]
        public ActionResult Delete(List<string> dealers)
        {
            try
            {
                _dealerService.DeleteDealer(dealers.ToArray());
                _dealerService.SaveDealer();
                return Json(new { Result = "Success" });
            }
            catch (Exception)
            {
                return Json(new { Result = "Failure" });
            }
        }
    }
}