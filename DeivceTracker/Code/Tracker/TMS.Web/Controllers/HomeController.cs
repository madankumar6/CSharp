using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using TMS.BusinessRule.Interfaces;
using TMS.DAL;
using TMS.Web.Models.ViewModels;
using TMS.Web.Rules;

namespace TMS.Web.Controllers
{
    public class Position
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }


    [TMSAuthorize]
    public class HomeController : Controller
    {
        IDealerService ds;
        private IDeviceInfoService dis;
        public HomeController(IDealerService ds, IDeviceInfoService dis)
        {
            this.ds = ds;
            this.dis = dis;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Error()
        {
            return View("Error");
        }
    }
}