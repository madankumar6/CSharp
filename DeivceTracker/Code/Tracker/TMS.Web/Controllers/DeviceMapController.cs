using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.DAL;
using TMS.Web.Models;

namespace TMS.Web.Controllers
{
    public class DeviceMapController : Controller
    {
        Models.TMSEntities entity = new Models.TMSEntities();

        // GET: DeviceMap
        public ActionResult Index()
        {
            return View(entity.deviceMap.ToList());
            
        }

        //public ActionResult Details(Guid id)
        //{
        //    DeviceMapping devicemap = entity.deviceMap.Find(id.ToString());
        //    if (devicemap == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(devicemap);
        //}
    }
}