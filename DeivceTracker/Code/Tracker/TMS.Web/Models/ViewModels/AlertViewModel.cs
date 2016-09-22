using DAL;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Tracker.Common;
using Tracker.Common.Model;

namespace TMS.Web.Models.ViewModels
{
    public class AlertReceiver
    {
        public int Id { get; set; }
        public string AlertId { get; set; }
        public string To { get; set; }
        public bool IsToDelete { get; set; }
        public AlertMediumType MediumType { get; set; }
    }

    public class ScheduleViewModel
    {
        public List<DeviceAlarmType> AlertTypes { get; set; }
        public List<string> Devices { get; set; }

        public DeviceAlarmType SelectedAlertType { get; set; } // 0 For all
        public string SelectedDevice { get; set; } // 0 For all

        public string SelectedAlertId { get; set; }

        public List<object> AlertDatas { get; set; }
    }

    // Basic
    public class AlertBase
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string DescriptionText { get; set; }

        public bool IsActive { get; set; }
        public List<Condition> Conditions { get; set; }
        public bool IsToDelete { get; set; }

        public List<ATFPosition> FenceList { get; set; }
        // Used to get from form values: Contains FenceList delimeter separated values
        public string FenceListStr { get; set; }
    }

     

    #region SeparateViewModel

    public class DeviceDetail
    {
        public string DeviceId { get; set; }
        public string VehicleId { get; set; }
    }

    public class DeviceDetailSelection
    {
        public string DeviceId { get; set; }
        public string VehicleId { get; set; }
        public bool Checked { get; set; }
    }

    public class DeviceDetails
    {
        public List<DeviceDetailSelection> Devices { get; set; }
    }

    public class AlertDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AlertCommonSetting : DeviceDetails
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class IgnitionAlert : AlertCommonSetting
    {
        public bool IsOnAcc { get; set; }
    }

    public class SpeedAlert : AlertCommonSetting
    {
        public int SpeedLimit { get; set; }
        public string SpeedComparer { get; set; }

        public List<DeviceDetail> Alerts { get; set; }
        public int SelectedAlert { get; set; }
    }

    public class StoppageAlert : AlertCommonSetting
    {
        public int Duration { get; set; }

        public List<DeviceDetail> Alerts { get; set; }
        public int SelectedAlert { get; set; }
    }

    public class MovingAlert : AlertCommonSetting
    {
        public int PeriodOfTime { get; set; }

        public List<DeviceDetail> Alerts { get; set; }
        public int SelectedAlert { get; set; }
    }

    public class FenceAlert : AlertCommonSetting
    {
        public DeviceAlarmType FenceType { get; set; }

        public List<ATFPosition> Points { get; set; }
        public string PointsStr { get; set; }

        public List<DeviceDetail> Alerts { get; set; }
        public int SelectedAlert { get; set; }
    }
    
    public class PowerCutAlert : AlertCommonSetting
    {
        public bool IsOnPowerCut { get; set; }

        public int SelectedAlert { get; set; }
    }

    public class SOSAlert : AlertCommonSetting
    {
        public bool IsOnSOS { get; set; }

        public int SelectedAlert { get; set; }
    }

    public class AcAlert : AlertCommonSetting
    {
        public bool IsOnAc { get; set; }

        public int SelectedAlert { get; set; }
    }
    #endregion

}