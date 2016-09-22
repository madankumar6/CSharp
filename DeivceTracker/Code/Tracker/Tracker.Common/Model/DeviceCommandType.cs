using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracker.Common.Model
{
    public enum DeviceCommandType
    {
        UnknownData = -1,
        None = 0,
        LoginData,
        LocationData,
        StatusInformation,

        AlarmData
    }

    public enum DeviceAlarmType
    {
        NormalAlarm = 0, // No alarm
        //REQ 2
        PowerCutAlarm,
        //REQ 1
        SOSAlarm,
        //REQ 1
        SpeedAlarm,
        BreakAlarm,
        VibrationAlarm,
        //REQ 1
        FenceAlarm,
        //REQ 1
        MovingAlarm,
        //REQ 1
        AccAlarm,
        //REQ 1
        StopAlarm,
        //REQ 1
        AcAlarm,
        FenceOutAlarm,
        FenceInAlarm,
    }
}