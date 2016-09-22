using System;
using System.Collections.Generic;
using System.Linq;
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
    [TMSAuthorize(Roles = "Admin,Distributor")]
    public class DistributorController : Controller
    {
        #region Properties
        private static readonly string FileName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType?.ToString();
        internal static Log4NetWrap Logger = new Log4NetWrap(FileName);

        private readonly IDistributorService _distributorService;
        #endregion

        public DistributorController(IDistributorService distributorService)
        {
            this._distributorService = distributorService;
        }

        public ActionResult GetDistributors(int draw, int start, int length)
        {
            if (start < 1)
                start = 1;
            else
                start = (start / length) + 1;

            

            //if (Session["UserData"] is Admin)
            //{
            Admin admin = Session["UserData"] as Admin;
            var distributors = _distributorService.GetDistributors(admin.UserId, start, length);
                var distributorsData = Mapper.Map<List<Distributor>, List<DistributorViewModel>>(distributors.Items).Select(dist => new { dist.UserId, dist.FirstName, dist.LastName, dist.Username, dist.PhoneNo, dist.Email });
        //   }
           // else
           // {
           //    distributors = _distributorService.GetDistributors(start, length);
           //  var distributorsData = Mapper.Map<List<Distributor>, List<DistributorViewModel>>(distributors.Items).Select(dist => new { dist.UserId, dist.FirstName, dist.LastName, dist.Username, dist.PhoneNo, dist.Email });
           //}


            return Json(new
            {
                draw = draw,
                recordsTotal = distributors.TotalItems,
                recordsFiltered = distributors.TotalItems,
                data = distributorsData
            }, JsonRequestBehavior.AllowGet);
        }

        // GET: Distributor
        public ActionResult Index()
        {
            return View();
        }

      
        // GET : /Distributor/Edit
        public ActionResult Edit(string distributorId)
        {
            DistributorViewModel distributorVM = new DistributorViewModel();
            if (string.IsNullOrWhiteSpace(distributorId))
            {
                return PartialView("_Edit", distributorVM);
            }
            else
            {
                var distributorGuid = Guid.Parse(distributorId);
                Distributor distributor = _distributorService.GetUser(distributorGuid);
                distributorVM = Mapper.Map<Distributor, DistributorViewModel>(distributor);

                return PartialView("_Edit", distributorVM);
            }
        }

        // POST : /Distributor/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DistributorViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Parent == Guid.Empty)
                {
                    // Add new
                    var parentUser = Session["UserData"] as User;
                    Distributor distributor = Mapper.Map<DistributorViewModel, Distributor>(model);
                    distributor.Parent = parentUser.UserId;
                    _distributorService.CreateDistributor(distributor);
                }
                else
                {
                    model.Password = "temp";
                    try
                    {
                        Distributor distributor = _distributorService.GetUser(model.UserId);
                        Mapper.Map<DistributorViewModel, Distributor>(model, distributor);
                        _distributorService.UpdateUser(distributor);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Error", ex.Message);
                    }
                }
                _distributorService.SaveDistributor();
            }
            else {
            }
            return Json(new { Result = "Success" });
        }


        // POST : /Distributor/Delete
        [HttpPost]
        public ActionResult Delete(List<string> distributors)
        {
            try
            {
                _distributorService.DeleteDistributor(distributors.ToArray());
                _distributorService.SaveDistributor();
                return Json(new { Result = "Success" });
            }
            catch (Exception)
            {
                return Json(new { Result = "Failure" });
            }
        }

        //public ActionResult ExportData()
        //{
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.Charset = "";
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.AddHeader("content-disposition", "attachment;filename= EmployeeReport.xlsx");

        //    using (MemoryStream MyMemoryStream = new MemoryStream())
        //    {
        //        wb.SaveAs(MyMemoryStream);
        //        MyMemoryStream.WriteTo(Response.OutputStream);
        //        Response.Flush();
        //        Response.End();
        //    }
        //    return RedirectToAction("Index", "ExportData");
        //}
    }
}