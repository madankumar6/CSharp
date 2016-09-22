using DAL;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Tracker.Common;
using Tracker.Common.Model;

namespace TMS.Web.Models.ViewModels
{
    public class ProtocolServerViewModel
    {
        public string ProtocolServer { get; set; }
        public int Port { get; set; }
        public int DevicesConnected { get; set; }
        public string Action { get; set; }
        public string ActionText { get; set; }
        public DateTime ActionTime { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}