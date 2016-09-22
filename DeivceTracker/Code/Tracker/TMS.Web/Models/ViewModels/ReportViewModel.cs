using DAL;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Tracker.Common;
using Tracker.Common.Model;

namespace TMS.Web.Models.ViewModels
{
    public class ReportParameterViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool OnAcc { get; set; }
        public string DeviceId { get; set; }
        public int Speed { get; set; }
        public string OnAccStr
        {
            get
            {
                if (this.OnAcc == true)
                {
                    return "On";
                }
                else
                {
                    return "Off";
                }
            }
        }
        public List<SelectListItem> DeviceList { get; set; }
    }

    public class ReportResultViewModel
    {
        public int SNo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool OnAcc { get; set; }
        public string OnAccStr
        {
            get
            {
                if (this.OnAcc == true)
                {
                    return "On";
                }
                else
                {
                    return "Off";
                }
            }
        }
        public string Lat { get; set; }
        public string Lang { get; set; }
        public int Speed { get; set; }
        public DateTime DeviceDataTime { get; set; }
        public string DeviceId { get; set; }
    }

    public class ReportViewModel {
        public ReportParameterViewModel Parameter { get; set; }
        public List<ReportResultViewModel> Results { get; set; }
    }
}