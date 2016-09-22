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
using TMS.Web.Models;
using Tracker.Common.Model;

namespace TMS.Web.Controllers
{
    public class AlertController : Controller
    {
        #region Properties
        private static readonly string FileName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType?.ToString();
        internal static Log4NetWrap Logger = new Log4NetWrap(FileName);
        #endregion

        //private readonly IDealerService _dealerService;

        //public AlertController(IDealerService dealerService)
        //{
        //    this._dealerService = dealerService;
        //}

        #region First

        public ActionResult Index()
        {
            return RedirectToAction("Schedule");
        }

        // GET : /Distributor/Add
        public ActionResult Schedule(string AlertType, string DeviceId, string AlertId)
        {
            ScheduleViewModel sVM = new ScheduleViewModel();
            sVM.AlertTypes = new AlertData().GetAvailableAlerts();
            sVM.Devices = new AlertData().GetAvailableDevices().Select(m=>m.DeviceId).ToList();

            if (!string.IsNullOrWhiteSpace(AlertType))
            {
                sVM.SelectedAlertType = (DeviceAlarmType)Enum.Parse(typeof(DeviceAlarmType), AlertType);
            }
            if (!string.IsNullOrWhiteSpace(DeviceId))
            {
                sVM.SelectedDevice = DeviceId;
            }
            if (!string.IsNullOrWhiteSpace(AlertId))
            {
                sVM.SelectedAlertId = AlertId;
            }

            if (!string.IsNullOrWhiteSpace(AlertType) && !string.IsNullOrWhiteSpace(DeviceId))
            {
                sVM = new AlertData().GetAlertData(sVM);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Schedule", sVM);
            }
            return View(sVM);
        }

        [HttpPost]
        public ActionResult SaveAlert(FormCollection form)
        {
            ScheduleViewModel model = new ScheduleViewModel();
            // Save the data
            UpdateModel(model, form);
            var tAlertData = new List<AlertBase>();
            TryUpdateModel(tAlertData, form);
            model.AlertDatas = Extensions.ToObjList<AlertBase>(tAlertData);

            new AlertData().SaveAlertData(model);

            return RedirectToAction("Schedule", new { AlertType = model.SelectedAlertType, DeviceId = model.SelectedDevice });
        }
        
        #endregion
    }
}