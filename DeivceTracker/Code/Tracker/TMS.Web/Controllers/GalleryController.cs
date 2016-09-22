using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.BusinessRule.Interfaces;
using TMS.Model;
using TMS.Web.Models.ViewModels;

namespace TMS.Web.Controllers
{
    public class GalleryController : Controller
    {
        #region
        private readonly IDealerService dealerService;
        private readonly IVehicleService<Vehicle> vehicleServce;
        private readonly IDeviceService<Device> deviceService;
        #endregion

        public GalleryController(IDealerService dealerService, IVehicleService<Vehicle> vehicleService,IDeviceService<Device> deviceService)
        {
            this.dealerService = dealerService;
            this.vehicleServce = vehicleService;
            this.deviceService = deviceService;
        }
        // GET: Gallery
        public ActionResult Index()
        {
            var deal = dealerService.GetDealers();
            var dealmap = Mapper.Map<List<Dealer>, List<DealerViewModel>>(deal);
            return View(dealmap);
        }

        public ActionResult GetDealers(int draw, int start, int length)
        {
            if (start < 1)
                start = 1;
            else
                start = (start / length) + 1;

            Admin admin = Session["UserData"] as Admin;
            var dealers = dealerService.GetDealers(start, length);
            var distributorsData = Mapper.Map<List<Dealer>, List<DealerViewModel>>(dealers.Items).Select(dist => new { dist.UserId, dist.FirstName, dist.LastName, dist.Username, dist.PhoneNo, dist.Logo });

            return Json(new
            {
                draw = draw,
                recordsTotal = dealers.TotalItems,
                recordsFiltered = dealers.TotalItems,
                data = distributorsData
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dealer()
        {
            // IEnumerable<Dealer> deal = dealerService.GetDealers();            
            var deal = dealerService.GetDealers();
            var dealmap = Mapper.Map<List<Dealer>, List<DealerViewModel>>(deal);
            return View(dealmap);
        }

        public ActionResult Vehicle()
        {
            var vehic = vehicleServce.GetVehicles();
            var vehicle = Mapper.Map<List<Vehicle>, List<VehicleViewModel>>(vehic);
            return View(vehicle);
        }
        public ActionResult Device()
        {
            var dev = deviceService.GetDevices();
            var device = Mapper.Map<List<Device>, List<DeviceViewModel>>(dev);
            return View(device);
        }
    }
}