using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Tracker.Common;
using Tracker.Common.Model;
using Utils;

namespace Tracker.Parser
{
    public abstract class Protocol
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

        public int Login_Data_LEN { get; set; }
        public int Location_Data_LEN { get; set; }
        public int Alarm_Data_LEN { get; set; }

        public abstract int REQ_MIN_Length();

        protected virtual DeviceInfo SetDataParsedTime(DeviceInfo di)
        {
            di.TrackerDataParsedTime = DateTime.UtcNow;
            return di;
        }

        protected decimal DegreeToDecimal(decimal Degrees, decimal Minutes, decimal Seconds)
        {
            return Degrees + (Minutes / 60) + (Seconds / 3600);
        }

        protected int GetNumber(string twoDigitHexString)
        {
            return Int32.Parse(twoDigitHexString, System.Globalization.NumberStyles.HexNumber);
        }

        protected string GetNumberString(string hexString)
        {
            string numberStr = "";
            while (hexString.Length > 1)
            {
                numberStr += GetNumber(hexString.Substring(0, 2)).ToString("00");
                hexString = hexString.Substring(2);
            }
            if (hexString.Length > 0)
            {
                numberStr += GetNumber(hexString).ToString();
            }
            return numberStr;
        }

        protected double GetDistance(string startLatitude, string startLongitude, string endLatitude, string endLongitude,
            int? prevSpeed, int? currSpeed)
        {
            return GetDistance((startLatitude.ToNullable<double>() ?? 0), (startLongitude.ToNullable<double>() ?? 0),
                (endLatitude.ToNullable<double>() ?? 0), (endLongitude.ToNullable<double>() ?? 0), prevSpeed ?? 0, currSpeed ?? 0);
        }
        private double GetDistance(double startLatitude, double startLongitude, double endLatitude, double endLongitude,
            int prevSpeed, int currSpeed)
        {
            try
            {
                if (prevSpeed == 0 && currSpeed == 0)
                {
                    return 0;
                }
                return (new GeoCoordinate(startLatitude, startLongitude)).GetDistanceTo(new GeoCoordinate(endLatitude, endLongitude));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        // Note: This method needs to handles the following:
        // Payload bytes may be only half due to connectivity issue
        // Payload bytes may contains 2 continuous string
        public abstract DeviceInfo Parse(DeviceInfo deviceInfo);
    }
}
