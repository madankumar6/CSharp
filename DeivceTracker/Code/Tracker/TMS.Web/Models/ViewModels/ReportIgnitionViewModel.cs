using DAL;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Tracker.Common;
using Tracker.Common.Model;

namespace TMS.Web.Models.ViewModels
{
    public class ReportIgnitionViewModel
    {
         public int RN { get; set; }
         public int OnAcc { get; set; }
         public  DateTime StartDate { get; set; }
         public DateTime StopDate { get; set; }
         public String StartLatitude { get; set; }
         public string StartLongitude { get; set; }
         public string StopLatitude { get; set; }
         public string StopLongitude { get; set; }
         public int Duration { get; set; }
     }
}