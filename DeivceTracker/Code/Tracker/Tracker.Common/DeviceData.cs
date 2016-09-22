using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Tracker.Common.Model;
using Utils;
using _DAL = DAL;

namespace Tracker.Common
{
    public class DeviceConnection
    {
        public string DeviceId { get; set; }
        public List<DeviceConnection> getconnection { get; set; }
        public SocketAsyncEventArgs e { get; set; }


    }
    public class DeviceData
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

        #region 
        public string DeviceId { get; set; }
        public string IMEI { get; set; }
        public string StatusCode { get; set; }
        // public Position Position { get; set; }
        //public string Speed { get; set; }
        //public string Direction { get; set; }
        //public string Altitude { get; set; }
        //public string Mileage { get; set; }
        //public string ValidData { get; set; }
        //public string FullAddress { get; set; }
        //public string Payload { get; set; }
        //public string UnParsedPayload { get; set; }
        //public string Distance { get; set; }
        //public string Odometer { get; set; }
        //public int? OnBattery { get; set; }
        //public int? OnIgnition { get; set; }
        //public int? OnAc { get; set; }
        //public int? OnGps { get; set; }
        //public string UnKnown { get; set; }
        //public DateTime? DeviceDataTime { get; set; }
        //public string GeozoneIndex { get; set; }
        //public string GeozoneID { get; set; }
        //public string TrackerIp { get; set; }

        //public DateTime? TrackerConnectedTime { get; set; }
        //public DateTime? TrackerDataActionTime { get; set; }
        //public DateTime? TrackerDataParsedTime { get; set; }

        //public DateTime ActionTime { get; set; }

        public static List<DeviceData> GetAllDevices()
        {
            List<DeviceData> DeviceDatas = new List<DeviceData>(){
                new DeviceData
                {
                    DeviceId = "9876ig6rr",
                    IMEI = "8978735",
                    StatusCode = "t77"
                }
            };
            return DeviceDatas;
        }
        #endregion

        private static DateTime? ActiveDeviceCachedOn = null;

        static DeviceData()
        {
            if (_DevicePool == null)
            {
                _DevicePool = GetDeviceDataFromDB();
            }
        }
        
        #region Active Devices

        public static List<string> _ActiveDevices = null;
        public static List<string> ActiveDevices
        {
            get
            {
                if (_ActiveDevices == null)
                {
                    _ActiveDevices = new List<string>();
                }

                if (ActiveDeviceCachedOn == null ||
                    // 10:00 < 09:50
                    // 10:00 < 09:55
                    // 10:00 < 10:00
                    ActiveDeviceCachedOn < DateTime.UtcNow.AddMinutes(-10))
                {
                    _ActiveDevices = GetActiveDevices();
                    ActiveDeviceCachedOn = DateTime.UtcNow;
                    //if (_InActiveDevices)
                    //{

                    //}
                    _ActiveDevices.ForEach(d =>
                    {
                        _UnknownDevices.Remove(d);
                    });
                }
                return _ActiveDevices;
            }
        }

        public static List<string> _UnknownDevices = new List<string>();

        public static DeviceInfo CopyLastDataOfDevice(DeviceInfo deviceInfo)
        {
            DeviceInfo di = GetDeviceById(deviceInfo.DeviceId);
            di.CommandType = deviceInfo.CommandType;
            di.ToSendRawData = deviceInfo.ToSendRawData;
            di.TrackerDataParsedTime = deviceInfo.TrackerDataParsedTime;
            di.ParserStatus = deviceInfo.ParserStatus;
            di.TrackerIp = deviceInfo.TrackerIp;
            di.DeviceDataTime = deviceInfo.DeviceDataTime;
            di.TrackerConnectedTime = deviceInfo.TrackerConnectedTime;
            di.TrackerDataActionTime = deviceInfo.TrackerDataActionTime;
            di.Payload = string.Empty;
            return di;
        }

        public static List<string> UnknownDevices
        {
            get
            {
                return _UnknownDevices;
            }
        }

        private static Mutex deviceMutex = new Mutex();
        private static Mutex unKnownDeviceMutex = new Mutex();
        private static Mutex devicePoolMutex = new Mutex();
        private static Mutex deviceConMutex = new Mutex();


        public static void AddActiveDevice(string DeviceId)
        {
            deviceMutex.WaitOne();
            if (!IsActiveDevice(DeviceId) && !IsUnknownDevice(DeviceId))
            {
                // Update to DB
                UpdateActiveDevice(DeviceId);
                ActiveDevices.Add(DeviceId);
            }
            deviceMutex.ReleaseMutex();
        }

        public static void AddUnknownDevice(string DeviceId)
        {
            unKnownDeviceMutex.WaitOne();
            if (UnknownDevices.IndexOf(DeviceId) < 0)
            {
                // Update to DB
                UpdateUnknownDevice(DeviceId);
                UnknownDevices.Add(DeviceId);
            }
            unKnownDeviceMutex.ReleaseMutex();
        }

        public static bool IsActiveDevice(string DeviceId)
        {
            return true;
            //return (ActiveDevices.IndexOf(DeviceId) >= 0);
        }

        public static bool IsUnknownDevice(string DeviceId)
        {
            return (UnknownDevices.IndexOf(DeviceId) >= 0);
        }



        public static List<DeviceInfo> _DevicePool = null;

        public static List<DeviceInfo> DevicePool
        {
            get
            {
                if (_DevicePool == null)
                {
                    _DevicePool = new List<DeviceInfo>();
                }
                return _DevicePool;
            }
        }

        public static DeviceInfo GetDeviceById(string deviceId)
        {
            DeviceInfo di = null;
            devicePoolMutex.WaitOne();
            {
                di = DevicePool.Where(d => d.DeviceId == deviceId).FirstOrDefault();
                if (di == null)
                {
                    di = new DeviceInfo()
                    {
                        DeviceId = deviceId
                    };
                    //DevicePool.Add(di);
                }

                di.ToSendRawData = new byte[0];
            }
            devicePoolMutex.ReleaseMutex();
            return di;
        }

        public static void UpdateDevice(DeviceInfo deviceInfo)
        {
            devicePoolMutex.WaitOne();
            {
                int index = DevicePool.FindIndex(d => d.DeviceId == deviceInfo.DeviceId);
                if (index < 0)
                {
                    DevicePool.Add(deviceInfo);
                }
                else
                {
                    DevicePool[index] = deviceInfo;
                }
            }
            devicePoolMutex.ReleaseMutex();
        }


        #region DB

        private static List<DeviceInfo> GetDeviceDataFromDB()
        {
            List<DeviceInfo> di = new List<DeviceInfo>();

            try
            {
                DataTable dt = _DAL.Data.GetData(_DAL.DataBase.TcpServer, System.Data.CommandType.StoredProcedure,
                    "T_GetDeviceData", null);
                di = dt.ToList<DeviceInfo>();
            }
            catch (Exception ex)
            {
            }
            return di;
        }

        private static List<string> GetActiveDevices()
        {
            List<string> devices = new List<string>();
            try
            {
                DataTable dt = _DAL.Data.GetData(_DAL.DataBase.TcpServer, System.Data.CommandType.StoredProcedure, "T_GetActiveDevices", null);
                if (dt != null && dt.Rows.Count > 0)
                {
                    devices = dt.AsEnumerable().Select(d => Convert.ToString(d["DeviceId"])).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return devices;
        }

        private static void UpdateUnknownDevice(string DeviceId)
        {
            var dbParams = new SqlParameter[]{
                    new SqlParameter("DeviceId", DeviceId)
            };

            try
            {
                _DAL.Data.StoreData_ExecuteNonQuery(_DAL.DataBase.TcpServer, System.Data.CommandType.StoredProcedure, "T_StoreUnknownDevice", dbParams);
            }
            catch (Exception ex)
            {
            }
        }

        private static void UpdateActiveDevice(string DeviceId)
        {
            //throw new NotImplementedException();
        }

        public static void UpdateDeviceOffline(string DeviceId)
        {
            var dbParams = new SqlParameter[]{
                    new SqlParameter("DeviceId", DeviceId),
                    new SqlParameter("IsOnline", false)
            };

            try
            {
                _DAL.Data.StoreData_ExecuteNonQuery(_DAL.DataBase.TcpServer, System.Data.CommandType.StoredProcedure, "T_StoreDeviceStatus", dbParams);
            }
            catch (Exception ex)
            {
            }
        }


        #endregion

        #endregion

        #region DeviceConnections

        public static List<DeviceConnection> _ActiveConnections = null;

        public static List<DeviceConnection> ActiveConnections
        {
            get
            {
                if (_ActiveConnections == null)
                {
                    _ActiveConnections = new List<DeviceConnection>();
                }
                return _ActiveConnections;
            }
        }

        public static void AddConnection(string DeviceId, SocketAsyncEventArgs con)
        {
            deviceConMutex.WaitOne();
            {
                ActiveConnections.Add(new DeviceConnection() { DeviceId = DeviceId, e = con });
            }
            deviceConMutex.ReleaseMutex();
        }

        public static List<SocketAsyncEventArgs> GetExistingConnectionsByDeviceId(string DeviceId)
        {
            deviceConMutex.WaitOne();
            List<SocketAsyncEventArgs> saeas = ActiveConnections.Where(m => m.DeviceId == DeviceId).Select(m => m.e).ToList();
            deviceConMutex.ReleaseMutex();
            return saeas;
        }

        public static int RemoveConnectionsByDeviceId(string DeviceId)
        {
            deviceConMutex.WaitOne();
            int RemovedConnections = 0;
            {
                RemovedConnections = ActiveConnections.RemoveAll(con => con.DeviceId == DeviceId);
            }
            deviceConMutex.ReleaseMutex();
            return RemovedConnections;
        }

        #endregion

        public static DeviceInfo ClearProcessedData(DeviceInfo deviceInfo)
        {
            log.DebugFormat("{0}/SaveData:", _fileNm);

            deviceInfo.CommandType = DeviceCommandType.None;
            //deviceInfo.StatusCode = string.Empty;
            //deviceInfo.Latitude = string.Empty;
            //deviceInfo.Longitude = string.Empty;
            //deviceInfo.Speed = string.Empty;
            //deviceInfo.Direction = string.Empty;
            //deviceInfo.Altitude = string.Empty;
            //deviceInfo.Mileage = string.Empty;
            //deviceInfo.ValidData = string.Empty;
            //deviceInfo.FullAddress = string.Empty;
            //deviceInfo.UnParsedPayload = string.Empty;
            //deviceInfo.Distance = string.Empty;
            deviceInfo.Odometer = 0;
            //deviceInfo.OnBattery = null;
            //deviceInfo.OnIgnition = null;
            //deviceInfo.OnAc = null;
            //deviceInfo.OnGps = null;
            //deviceInfo.UnKnown = string.Empty;
            //deviceInfo.GeozoneIndex = string.Empty;
            //deviceInfo.GeozoneID = string.Empty;
            //deviceInfo.DeviceDataTime = null;
            //deviceInfo.GeozoneIndex = null;
            //deviceInfo.GeozoneID = null;
            //deviceInfo.TrackerIp = null;
            //deviceInfo.TrackerDataActionTime = null;
            //deviceInfo.TrackerDataParsedTime = null;

            deviceInfo.ReplyMessage = null;

            deviceInfo.RawData = new byte[0];
            //deviceInfo.ToSendRawData = null;

            deviceInfo.ParserStatus = ProtocolParserStatus.Initialized;

            return deviceInfo;
        }

        public static DeviceInfo Dispose(DeviceInfo deviceInfo)
        {
            deviceInfo.DeviceId = null;
            deviceInfo.IMEI = null;
            deviceInfo.CommandType = DeviceCommandType.None;
            deviceInfo.StatusCode = null;
            deviceInfo.Latitude = null;
            deviceInfo.Longitude = null;
            deviceInfo.Speed = null;
            deviceInfo.Direction = null;
            deviceInfo.Altitude = null;
            deviceInfo.Mileage = null;
            deviceInfo.ValidData = null;
            deviceInfo.FullAddress = null;
            deviceInfo.Payload = null;
            deviceInfo.UnParsedPayload = null;
            deviceInfo.Distance = null;
            deviceInfo.Odometer = null;
            deviceInfo.OnLowBattery = null;
            deviceInfo.OnIgnition = null;
            deviceInfo.OnAc = null;
            deviceInfo.OnGps = null;
            deviceInfo.UnKnown = null;
            deviceInfo.DeviceDataTime = null;
            deviceInfo.GeozoneIndex = null;
            deviceInfo.GeozoneID = null;
            deviceInfo.TrackerIp = null;
            deviceInfo.TrackerConnectedTime = null;
            deviceInfo.TrackerDataActionTime = null;
            deviceInfo.TrackerDataParsedTime = null;


            deviceInfo.ReplyMessage = null;

            deviceInfo.RawData = new byte[0];
            deviceInfo.ToSendRawData = null;

            deviceInfo.ParserStatus = ProtocolParserStatus.Initialized;
            return deviceInfo;
        }

        public static void UpdateNumOfDevicesConnected(string Protocol, int Port, int Count)
        {
            try
            {
                var dbParams = new SqlParameter[]{
                    new SqlParameter("Protocol", Protocol),
                    new SqlParameter("Port", Port),
                    new SqlParameter("Count", Count)
                };

                _DAL.Data.StoreData_ExecuteNonQuery(_DAL.DataBase.TcpServer, System.Data.CommandType.StoredProcedure, "T_StoreNumOfDevicesConnected", dbParams);

            }
            catch (Exception ex)
            {
            }
        }

        public static bool SaveData(DeviceInfo deviceInfo)
        {
            try
            {
                log.DebugFormat("{0}/SaveData:", _fileNm);

                var dbParams = new SqlParameter[]{
                    new SqlParameter("DeviceId", deviceInfo.DeviceId),
                    new SqlParameter("IMEI", deviceInfo.IMEI),
                    new SqlParameter("CommandType", deviceInfo.CommandType),
                    new SqlParameter("StatusCode", deviceInfo.StatusCode),
                    new SqlParameter("Latitude", deviceInfo.Latitude),
                    new SqlParameter("Longitude", deviceInfo.Longitude),
                    new SqlParameter("Speed", deviceInfo.Speed),
                    new SqlParameter("Direction", deviceInfo.Direction),
                    new SqlParameter("Altitude", deviceInfo.Altitude),
                    new SqlParameter("Mileage", deviceInfo.Mileage),
                    new SqlParameter("ValidData", deviceInfo.ValidData),
                    new SqlParameter("FullAddress", deviceInfo.FullAddress),
                    new SqlParameter("PayLoad", deviceInfo.Payload),
                    new SqlParameter("UnParsedPayload", deviceInfo.UnParsedPayload),
                    new SqlParameter("Distance", deviceInfo.Distance),
                    new SqlParameter("Odometer", deviceInfo.Odometer),
                    new SqlParameter("OnBattery", deviceInfo.OnLowBattery),
                    new SqlParameter("OnIgnition", deviceInfo.OnIgnition),
                    new SqlParameter("OnAc", deviceInfo.OnAc),
                    new SqlParameter("OnGps", deviceInfo.OnGps),
                    new SqlParameter("UnKnown", deviceInfo.UnKnown),
                    new SqlParameter("GeozoneIndex", deviceInfo.GeozoneIndex),
                    new SqlParameter("GeozoneID", deviceInfo.GeozoneID),
                    new SqlParameter("TrackerIp", deviceInfo.TrackerIp),
                    new SqlParameter("DeviceDataTime", deviceInfo.DeviceDataTime),
                    new SqlParameter("TrackerConnectedTime", deviceInfo.TrackerConnectedTime),
                    new SqlParameter("TrackerDataActionTime", deviceInfo.TrackerDataActionTime),
                    new SqlParameter("TrackerDataParsedTime", deviceInfo.TrackerDataParsedTime),


                    new SqlParameter("OnAcc", deviceInfo.OnAcc),
                    new SqlParameter("OilNElectricConected", deviceInfo.OilNElectricConected),
                    new SqlParameter("OnSOS", deviceInfo.OnSOS),
                    new SqlParameter("OnLowBattery", deviceInfo.OnLowBattery),
                    new SqlParameter("OnPowerCut", deviceInfo.OnPowerCut),
                    new SqlParameter("OnShock", deviceInfo.OnShock),
                    new SqlParameter("OnCharge", deviceInfo.OnCharge),
                    new SqlParameter("OnDefence", deviceInfo.OnDefence),
                    new SqlParameter("VoltageLevel", deviceInfo.VoltageLevel),
                    new SqlParameter("SignalStrengthLevel", deviceInfo.SignalStrengthLevel),
                    new SqlParameter("AlarmType", deviceInfo.AlarmType)
                };

                _DAL.Data.StoreData_ExecuteNonQuery(_DAL.DataBase.TcpServer, System.Data.CommandType.StoredProcedure, "T_StoreDeviceData", dbParams);

                log.InfoFormat("{0}/SaveData: Finished", _fileNm);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("{0}/SaveData: {1}", _fileNm, ex);
            }
            return true;
        }

    }
}
