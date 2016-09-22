using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Tracker.Common;
using Tracker.Common.Model;
using Utils;

namespace Alertify
{
    public class AlertifyMain
    {

        int ProcessorId;

        public AlertifyMain(int ProcessorId)
        {
            this.ProcessorId = ProcessorId;
        }

        #region FireAlerts

        public void Start()
        {

            // Get identical record for each device
            List<ATDevice> devices = GetDeviceAlerts(this.ProcessorId);
            // Get active alerts for each record
            devices.ForEach(device =>
            {
                // For each alert
                device.Alerts.ForEach(alert =>
                {
                    try
                    {
                        if (
                            alert.AlarmType != DeviceAlarmType.FenceAlarm &&
                            alert.AlarmType != DeviceAlarmType.FenceInAlarm &&
                            alert.AlarmType != DeviceAlarmType.FenceOutAlarm
                            )
                        {
                            #region Table valued alarm
                            //  Parse current data which is applicable for this alert
                            //  Parse previous state data which is parsed in previous loop

                            // If alert condition is satisified 
                            if (ConditionProcessor.ProcessConditionWithOutDuration(alert))
                            {   //  Process to send alert
                                if (alert.IsSent == false)
                                {
                                    // If duration is in condition, log the current condition state
                                    if (alert.Conditions.Where(m => m.Operand.ToLower() == "Duration".ToLower()).ToList().Count == 0 ||
                                    ConditionProcessor.ProcessDurationOnCondition(alert))
                                    {
                                        // ** Becareful on edit this condition, first part always run, second part will run only the first fails
                                        FireAlertWData(device.Id, alert);
                                        alert.IsSent = true;
                                        alert.SentTime = DateTime.UtcNow;
                                    }
                                }
                                if (alert.ConditionState == false)
                                {
                                    alert.ConditionState = true;
                                    alert.ConditionStateTime = DateTime.UtcNow;
                                }
                            }
                            else
                            {
                                // If duration is in condition, log the current condition state

                                // Reset alert log
                                alert.IsSent = false;

                                if (alert.ConditionState == true)
                                {
                                    alert.ConditionState = false;
                                    alert.ConditionStateTime = DateTime.UtcNow;
                                }
                            }
                            //alert.PreviousData = alert.CurrentData;
                            #endregion
                        }
                        else
                        {
                            #region Fence oriented alarm
                            // Get Fence list
                            alert.FencePosition = GetFenceList(alert.Id);

                            if (ConditionProcessor.ProcessFenceConditionWithOutDuration(alert))
                            {   //  Process to send alert
                                if (alert.IsSent == false)
                                {
                                    // If duration is in condition, log the current condition state
                                    if (alert.Conditions.Where(m => m.Operand.ToLower() == "Duration".ToLower()).ToList().Count == 0 ||
                                    ConditionProcessor.ProcessDurationOnCondition(alert))
                                    {
                                        // ** Becareful on edit this condition, first part always run, second part will run only the first fails
                                        FireAlertWData(device.Id, alert);
                                        alert.IsSent = true;
                                        alert.SentTime = DateTime.UtcNow;
                                    }
                                }
                                if (alert.ConditionState == false)
                                {
                                    alert.ConditionState = true;
                                    alert.ConditionStateTime = DateTime.UtcNow;
                                }
                            }
                            else
                            {
                                // If duration is in condition, log the current condition state

                                // Reset alert log
                                alert.IsSent = false;

                                if (alert.ConditionState == true)
                                {
                                    alert.ConditionState = false;
                                    alert.ConditionStateTime = DateTime.UtcNow;
                                }
                            }

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                });
            });

            // Store current state as previous data
            StoreDeviceAlertCurrentData(devices);

            devices = null;
        }

        private List<ATDevice> GetDeviceAlerts(int ProcessorId)
        {
            // TODO have to point to DAT table to process each & every record updated, instead of processing the current record
            List<ATDevice> devices = new List<ATDevice>();

            DataTable dt = new DataTable();
            try
            {

                dt = Data.GetData(DataBase.Alert, CommandType.StoredProcedure, "A_GetDevicesNAlerts", new SqlParameter[] { new SqlParameter("@ProcessorId", ProcessorId) });

                devices = dt.AsEnumerable().GroupBy(dG => dG["DeviceId"]).Select(row =>
                    new ATDevice
                    {
                        Id = row.Key.ToString(),
                        Alerts = row.Select(dRow => new ATAlert()
                        {
                            Id = Convert.ToInt32(dRow["DeviceAlertId"]),
                            IsSent = Convert.ToBoolean(dRow["IsSent"]),
                            SentTime = Convert.ToDateTime(dRow["SentTime"]),
                            AlarmType = (Tracker.Common.Model.DeviceAlarmType)dRow["AlertType"],
                            Eval = Convert.ToString(dRow["Eval"]),
                            Conditions = AlertData.DeSerializeCondition(Convert.ToString(dRow["Eval"])),
                            ConditionState = Convert.ToBoolean(dRow["ConditionState"]),
                            ConditionStateTime = Convert.ToDateTime(dRow["ConditionStateTime"]),
                            CurrentData = new ATData()
                            {
                                VariableNVales = (new KeyValuePair<string, string>[] {
                                 new KeyValuePair<string, string>("CommandType", Convert.ToString(dRow["CommandType"])),
                                 new KeyValuePair<string, string>("StatusCode", Convert.ToString(dRow["StatusCode"])),
                                 new KeyValuePair<string, string>("Latitude", Convert.ToString(dRow["Latitude"])),
                                 new KeyValuePair<string, string>("Longitude", Convert.ToString(dRow["Longitude"])),
                                 new KeyValuePair<string, string>("Speed", Convert.ToString(dRow["Speed"])),
                                 new KeyValuePair<string, string>("Direction", Convert.ToString(dRow["Direction"])),
                                 new KeyValuePair<string, string>("OnBattery", Convert.ToString(dRow["OnBattery"])),
                                 new KeyValuePair<string, string>("OnIgnition", Convert.ToString(dRow["OnIgnition"])),
                                 new KeyValuePair<string, string>("OnAc", Convert.ToString(dRow["OnAc"])),
                                 new KeyValuePair<string, string>("OnGps", Convert.ToString(dRow["OnGps"])),
                                 new KeyValuePair<string, string>("OnAcc", Convert.ToString(dRow["OnAcc"])),
                                 new KeyValuePair<string, string>("OilNElectricConected", Convert.ToString(dRow["OilNElectricConected"])),
                                 new KeyValuePair<string, string>("OnSOS", Convert.ToString(dRow["OnSOS"])),
                                 new KeyValuePair<string, string>("OnLowBattery", Convert.ToString(dRow["OnLowBattery"])),
                                 new KeyValuePair<string, string>("OnPowerCut", Convert.ToString(dRow["OnPowerCut"])),
                                 new KeyValuePair<string, string>("OnShock", Convert.ToString(dRow["OnShock"])),
                                 new KeyValuePair<string, string>("OnCharge", Convert.ToString(dRow["OnCharge"])),
                                 new KeyValuePair<string, string>("OnDefence", Convert.ToString(dRow["OnDefence"])),
                                 new KeyValuePair<string, string>("AlarmType", Convert.ToString(dRow["AlarmType"])),
                                 new KeyValuePair<string, string>("DeviceDataTime", Convert.ToString(dRow["DeviceDataTime"])),
                                 new KeyValuePair<string, string>("TrackerConnectedTime", Convert.ToString(dRow["TrackerConnectedTime"])),
                                 new KeyValuePair<string, string>("TrackerDataActionTime", Convert.ToString(dRow["TrackerDataActionTime"])),
                                 new KeyValuePair<string, string>("TrackerDataParsedTime", Convert.ToString(dRow["TrackerDataParsedTime"])),
                                 new KeyValuePair<string, string>("ActionTime", Convert.ToString(dRow["ActionTime"])),
                                 new KeyValuePair<string, string>("Duration", DateTime.UtcNow.Subtract(Convert.ToDateTime(dRow["ConditionStateTime"])).TotalMinutes.ToString()),

                                 new KeyValuePair<string, string>("CurrentDate", (Convert.ToDouble(dRow["CurrentDate"])).ToString()),
                                 new KeyValuePair<string, string>("CurrentTime", (Convert.ToDouble(dRow["CurrentTime"])).ToString())
                                }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                            },
                            PreviousData = new ATData()
                            {
                                VariableNVales = (new KeyValuePair<string, string>[] {
                                 new KeyValuePair<string, string>("CommandType", Convert.ToString(dRow["PrevCommandType"])),
                                 new KeyValuePair<string, string>("StatusCode", Convert.ToString(dRow["PrevStatusCode"])),
                                 new KeyValuePair<string, string>("Latitude", Convert.ToString(dRow["PrevLatitude"])),
                                 new KeyValuePair<string, string>("Longitude", Convert.ToString(dRow["PrevLongitude"])),
                                 new KeyValuePair<string, string>("Speed", Convert.ToString(dRow["PrevSpeed"])),
                                 new KeyValuePair<string, string>("Direction", Convert.ToString(dRow["PrevDirection"])),
                                 new KeyValuePair<string, string>("OnBattery", Convert.ToString(dRow["PrevOnBattery"])),
                                 new KeyValuePair<string, string>("OnIgnition", Convert.ToString(dRow["PrevOnIgnition"])),
                                 new KeyValuePair<string, string>("OnAc", Convert.ToString(dRow["PrevOnAc"])),
                                 new KeyValuePair<string, string>("OnGps", Convert.ToString(dRow["PrevOnGps"])),
                                 new KeyValuePair<string, string>("OnAcc", Convert.ToString(dRow["PrevOnAcc"])),
                                 new KeyValuePair<string, string>("OilNElectricConected", Convert.ToString(dRow["PrevOilNElectricConected"])),
                                 new KeyValuePair<string, string>("OnSOS", Convert.ToString(dRow["PrevOnSOS"])),
                                 new KeyValuePair<string, string>("OnLowBattery", Convert.ToString(dRow["PrevOnLowBattery"])),
                                 new KeyValuePair<string, string>("OnPowerCut", Convert.ToString(dRow["PrevOnPowerCut"])),
                                 new KeyValuePair<string, string>("OnShock", Convert.ToString(dRow["PrevOnShock"])),
                                 new KeyValuePair<string, string>("OnCharge", Convert.ToString(dRow["PrevOnCharge"])),
                                 new KeyValuePair<string, string>("OnDefence", Convert.ToString(dRow["PrevOnDefence"])),
                                 new KeyValuePair<string, string>("AlarmType", Convert.ToString(dRow["PrevAlarmType"])),
                                 new KeyValuePair<string, string>("DeviceDataTime", Convert.ToString(dRow["PrevDeviceDataTime"])),
                                 new KeyValuePair<string, string>("TrackerConnectedTime", Convert.ToString(dRow["PrevTrackerConnectedTime"])),
                                 new KeyValuePair<string, string>("TrackerDataActionTime", Convert.ToString(dRow["PrevTrackerDataActionTime"])),
                                 new KeyValuePair<string, string>("TrackerDataParsedTime", Convert.ToString(dRow["PrevTrackerDataParsedTime"])),
                                 new KeyValuePair<string, string>("ActionTime", Convert.ToString(dRow["PrevActionTime"]))
                                }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                            }
                        }).ToList()

                    }
                ).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                dt = null;
            }
            return devices;
        }

        private List<ATFPosition> GetFenceList(int DeviceAlertId)
        {
            List<ATFPosition> FencePosition = new List<ATFPosition>();

            DataTable dt = new DataTable();

            try
            {
                dt = Data.GetData(DataBase.Alert, CommandType.StoredProcedure, "A_GetFenceList",
                    new SqlParameter[] { new SqlParameter("DeviceAlertId", DeviceAlertId) });

                FencePosition = dt.AsEnumerable().Select(m => new ATFPosition()
                {
                    Lat = Convert.ToString(m["Latitude"]),
                    Lang = Convert.ToString(m["Longitude"]),
                    //Distance = (float)Convert.ToDouble(m["Distance"])
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                dt = null;
            }

            return FencePosition;
        }

        private void FireAlertWData(string DeviceId, ATAlert alert)
        {
            {
                DataTable AlertDataFromService = new DataTable();
                try
                {

                    #region Column Definition
                    AlertDataFromService.Columns.Add("DeviceId", typeof(string));
                    AlertDataFromService.Columns.Add("IMEI", typeof(string));
                    AlertDataFromService.Columns.Add("CommandType", typeof(string));
                    AlertDataFromService.Columns.Add("StatusCode", typeof(string));
                    AlertDataFromService.Columns.Add("Latitude", typeof(string));
                    AlertDataFromService.Columns.Add("Longitude", typeof(string));
                    AlertDataFromService.Columns.Add("Speed", typeof(string));
                    AlertDataFromService.Columns.Add("Direction", typeof(string));
                    AlertDataFromService.Columns.Add("Altitude", typeof(string));
                    AlertDataFromService.Columns.Add("Mileage", typeof(string));
                    AlertDataFromService.Columns.Add("Odometer", typeof(int));
                    AlertDataFromService.Columns.Add("OnBattery", typeof(int));
                    AlertDataFromService.Columns.Add("OnIgnition", typeof(int));
                    AlertDataFromService.Columns.Add("OnAc", typeof(int));
                    AlertDataFromService.Columns.Add("OnGps", typeof(int));
                    AlertDataFromService.Columns.Add("OnAcc", typeof(int));
                    AlertDataFromService.Columns.Add("OilNElectricConected", typeof(int));
                    AlertDataFromService.Columns.Add("OnSOS", typeof(int));
                    AlertDataFromService.Columns.Add("OnLowBattery", typeof(int));
                    AlertDataFromService.Columns.Add("OnPowerCut", typeof(int));
                    AlertDataFromService.Columns.Add("OnShock", typeof(int));
                    AlertDataFromService.Columns.Add("OnCharge", typeof(int));
                    AlertDataFromService.Columns.Add("OnDefence", typeof(int));
                    AlertDataFromService.Columns.Add("VoltageLevel", typeof(int));
                    AlertDataFromService.Columns.Add("SignalStrengthLevel", typeof(int));
                    AlertDataFromService.Columns.Add("AlarmType", typeof(string));
                    AlertDataFromService.Columns.Add("TrackerIp", typeof(string));
                    AlertDataFromService.Columns.Add("DeviceDataTime", typeof(DateTime));
                    AlertDataFromService.Columns.Add("TrackerConnectedTime", typeof(DateTime));
                    AlertDataFromService.Columns.Add("TrackerDataActionTime", typeof(DateTime));
                    AlertDataFromService.Columns.Add("TrackerDataParsedTime", typeof(DateTime));
                    AlertDataFromService.Columns.Add("ActionTime", typeof(DateTime));

                    AlertDataFromService.Columns.Add("DeviceAlertId", typeof(int));
                    AlertDataFromService.Columns.Add("IsSent", typeof(bool));
                    AlertDataFromService.Columns.Add("SentTime", typeof(DateTime));

                    AlertDataFromService.Columns.Add("ConditionState", typeof(bool));
                    AlertDataFromService.Columns.Add("ConditionStateTime", typeof(DateTime));
                    #endregion

                    {

                        DataRow dr = AlertDataFromService.Rows.Add(
                            DeviceId,
                            null,
                            alert.CurrentData.VariableNVales["CommandType"],
                            alert.CurrentData.VariableNVales["StatusCode"],
                            alert.CurrentData.VariableNVales["Latitude"],
                            alert.CurrentData.VariableNVales["Longitude"],
                            alert.CurrentData.VariableNVales["Speed"],
                            alert.CurrentData.VariableNVales["Direction"],
                            null,
                            null,
                            null,
                            alert.CurrentData.VariableNVales["OnBattery"],
                            alert.CurrentData.VariableNVales["OnIgnition"],
                            alert.CurrentData.VariableNVales["OnAc"],
                            alert.CurrentData.VariableNVales["OnGps"],
                            alert.CurrentData.VariableNVales["OnAcc"],
                            alert.CurrentData.VariableNVales["OilNElectricConected"],
                            alert.CurrentData.VariableNVales["OnSOS"],
                            alert.CurrentData.VariableNVales["OnLowBattery"],
                            alert.CurrentData.VariableNVales["OnPowerCut"],
                            alert.CurrentData.VariableNVales["OnShock"],
                            alert.CurrentData.VariableNVales["OnCharge"],
                            alert.CurrentData.VariableNVales["OnDefence"],
                            null,
                            null,
                            alert.CurrentData.VariableNVales["AlarmType"],
                            null,
                            alert.CurrentData.VariableNVales["DeviceDataTime"],
                            (string.IsNullOrWhiteSpace(alert.CurrentData.VariableNVales["TrackerConnectedTime"]) ? (DateTime?)null : Convert.ToDateTime(alert.CurrentData.VariableNVales["TrackerConnectedTime"])),
                            alert.CurrentData.VariableNVales["TrackerDataActionTime"],
                            alert.CurrentData.VariableNVales["TrackerDataParsedTime"],
                            alert.CurrentData.VariableNVales["ActionTime"],

                            alert.Id,
                            alert.IsSent,
                            alert.SentTime,

                            alert.ConditionState,
                            alert.ConditionStateTime
                        );
                    }

                    List<TVParameter> TVParameters = new List<TVParameter>();
                    TVParameters.Add(new TVParameter()
                    {
                        ParameterName = "@AlertDataFromService",
                        ParameterValue = AlertDataFromService,
                        ParameterTypeName = "DeviceAlertDataType_V1",
                        SqlDbType = SqlDbType.Structured
                    });

                    Data.StoreData_ExecuteNonQuery(DataBase.Alert, CommandType.StoredProcedure, "A_StoreDeviceFiredAlerts", new SqlParameter[] {
                        new SqlParameter("DeviceAlertId", alert.Id),
                        new SqlParameter("AlertType", alert.AlarmType),
                        new SqlParameter("Eval", alert.Eval),
                        new SqlParameter("Status", alert.IsSent),
                        new SqlParameter("ProcessorId", this.ProcessorId)
                    }, TVParameters);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    AlertDataFromService = null;
                }
            }
        }

        private void StoreDeviceAlertCurrentData(List<ATDevice> devices)
        {
            DataTable AlertDataFromService = new DataTable();
            try
            {

                #region Column Definition
                AlertDataFromService.Columns.Add("DeviceId", typeof(string));
                AlertDataFromService.Columns.Add("IMEI", typeof(string));
                AlertDataFromService.Columns.Add("CommandType", typeof(string));
                AlertDataFromService.Columns.Add("StatusCode", typeof(string));
                AlertDataFromService.Columns.Add("Latitude", typeof(string));
                AlertDataFromService.Columns.Add("Longitude", typeof(string));
                AlertDataFromService.Columns.Add("Speed", typeof(string));
                AlertDataFromService.Columns.Add("Direction", typeof(string));
                AlertDataFromService.Columns.Add("Altitude", typeof(string));
                AlertDataFromService.Columns.Add("Mileage", typeof(string));
                AlertDataFromService.Columns.Add("Odometer", typeof(int));
                AlertDataFromService.Columns.Add("OnBattery", typeof(int));
                AlertDataFromService.Columns.Add("OnIgnition", typeof(int));
                AlertDataFromService.Columns.Add("OnAc", typeof(int));
                AlertDataFromService.Columns.Add("OnGps", typeof(int));
                AlertDataFromService.Columns.Add("OnAcc", typeof(int));
                AlertDataFromService.Columns.Add("OilNElectricConected", typeof(int));
                AlertDataFromService.Columns.Add("OnSOS", typeof(int));
                AlertDataFromService.Columns.Add("OnLowBattery", typeof(int));
                AlertDataFromService.Columns.Add("OnPowerCut", typeof(int));
                AlertDataFromService.Columns.Add("OnShock", typeof(int));
                AlertDataFromService.Columns.Add("OnCharge", typeof(int));
                AlertDataFromService.Columns.Add("OnDefence", typeof(int));
                AlertDataFromService.Columns.Add("VoltageLevel", typeof(int));
                AlertDataFromService.Columns.Add("SignalStrengthLevel", typeof(int));
                AlertDataFromService.Columns.Add("AlarmType", typeof(string));
                AlertDataFromService.Columns.Add("TrackerIp", typeof(string));
                AlertDataFromService.Columns.Add("DeviceDataTime", typeof(DateTime));
                AlertDataFromService.Columns.Add("TrackerConnectedTime", typeof(DateTime));
                AlertDataFromService.Columns.Add("TrackerDataActionTime", typeof(DateTime));
                AlertDataFromService.Columns.Add("TrackerDataParsedTime", typeof(DateTime));
                AlertDataFromService.Columns.Add("ActionTime", typeof(DateTime));


                AlertDataFromService.Columns.Add("DeviceAlertId", typeof(int));
                AlertDataFromService.Columns.Add("IsSent", typeof(bool));
                AlertDataFromService.Columns.Add("SentTime", typeof(DateTime));

                AlertDataFromService.Columns.Add("ConditionState", typeof(bool));
                AlertDataFromService.Columns.Add("ConditionStateTime", typeof(DateTime));
                #endregion

                foreach (var device in devices)
                {
                    foreach (var alert in device.Alerts)
                    {
                        DataRow dr = AlertDataFromService.Rows.Add(
                            device.Id,
                            null,
                            alert.CurrentData.VariableNVales["CommandType"],
                            alert.CurrentData.VariableNVales["StatusCode"],
                            alert.CurrentData.VariableNVales["Latitude"],
                            alert.CurrentData.VariableNVales["Longitude"],
                            alert.CurrentData.VariableNVales["Speed"],
                            alert.CurrentData.VariableNVales["Direction"],
                            null,
                            null,
                            null,
                            alert.CurrentData.VariableNVales["OnBattery"],
                            alert.CurrentData.VariableNVales["OnIgnition"],
                            alert.CurrentData.VariableNVales["OnAc"],
                            alert.CurrentData.VariableNVales["OnGps"],
                            alert.CurrentData.VariableNVales["OnAcc"],
                            alert.CurrentData.VariableNVales["OilNElectricConected"],
                            alert.CurrentData.VariableNVales["OnSOS"],
                            alert.CurrentData.VariableNVales["OnLowBattery"],
                            alert.CurrentData.VariableNVales["OnPowerCut"],
                            alert.CurrentData.VariableNVales["OnShock"],
                            alert.CurrentData.VariableNVales["OnCharge"],
                            alert.CurrentData.VariableNVales["OnDefence"],
                            null,
                            null,
                            alert.CurrentData.VariableNVales["AlarmType"],
                            null,
                            alert.CurrentData.VariableNVales["DeviceDataTime"],
                            (string.IsNullOrWhiteSpace(alert.CurrentData.VariableNVales["TrackerConnectedTime"]) ? (DateTime?)null : Convert.ToDateTime(alert.CurrentData.VariableNVales["TrackerConnectedTime"])),
                            alert.CurrentData.VariableNVales["TrackerDataActionTime"],
                            alert.CurrentData.VariableNVales["TrackerDataParsedTime"],
                            alert.CurrentData.VariableNVales["ActionTime"],

                            alert.Id,
                            alert.IsSent,
                            alert.SentTime,

                            alert.ConditionState,
                            alert.ConditionStateTime
                        );
                    }
                }

                // 

                List<TVParameter> TVParameters = new List<TVParameter>();
                TVParameters.Add(new TVParameter()
                {
                    ParameterName = "@AlertDataFromService",
                    ParameterValue = AlertDataFromService,
                    ParameterTypeName = "DeviceAlertDataType_V1",
                    SqlDbType = SqlDbType.Structured
                });

                Data.StoreData_ExecuteNonQuery(DataBase.Alert, CommandType.StoredProcedure, "A_StoreDevicesNAlerts", new SqlParameter[] { new SqlParameter("ProcessorId", this.ProcessorId) }, TVParameters);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                AlertDataFromService = null;
            }
        }

        #endregion


        #region FiredAlerts

        public void ProcessFiredAlerts()
        {
            // Get Generated alerts
            List<ATDevice> generatedAlerts = GetFiredAlerts(this.ProcessorId);

            generatedAlerts.ForEach(d =>
            {
                ATCustomerDetail customerDetail = new ATCustomerDetail();
                customerDetail = GetCustomerDetails(d.Id);

                if (customerDetail != null && customerDetail.IsAccountExpired == false)
                {
                    // Check for the particular customer is having message count (Email/SMS)
                    d.Alerts.ForEach(alert =>
                    {
                        try
                        {
                            // Get templates by its alert type from Global cached variable
                            AlertDelivery ed = new AlertDelivery();

                            // Get Email lists
                            ed.ToAddresses = GetFiredAlertsReceivers(alert.Id);
                            ed.ToAddresses = ed.ToAddresses ?? new List<AlertDeliveryNStatus>();

                            // TODO Get SMS lists

                            if (ed.ToAddresses.Count > 0)
                            {
                                try
                                {
                                    if (ed.ToAddresses.Where(m => m.MediumType == AlertMediumType.Email).ToList().Count > 0
                                    && customerDetail.EMAILBalance > 0)
                                    {
                                        ed.EmailContent = TemplateParser.GetEmailTemplateWithValues(alert.AlarmType, d);
                                        ed.SmtpSettings = new SmtpSettings();
                                        ed = new EmailAlert().SendMail(ed);
                                        customerDetail.EMAILBalance = customerDetail.EMAILBalance - ed.ToAddresses.Select(m => m.MediumType == AlertMediumType.Email && m.SentStatus == DeliveryStatus.Success).Count();
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                try
                                {
                                    if (ed.ToAddresses.Where(m => m.MediumType == AlertMediumType.SMS).ToList().Count > 0
                                    && customerDetail.SMSBalance > 0)
                                    {
                                        ed.SmsContent = TemplateParser.GetSmsTemplateWithValues(alert.AlarmType, d);
                                        ed.SMSApiSettings = new SMSApiSettings();
                                        ed = new SmsAlert().SendMessage(ed);
                                        customerDetail.SMSBalance = customerDetail.SMSBalance - ed.ToAddresses.Select(m => m.MediumType == AlertMediumType.SMS && m.SentStatus == DeliveryStatus.Success).Count();
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                StoreDeliverLog(d.Id, alert.Id, alert.AlarmType, ed);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    });

                    SaveCustomerDetails(customerDetail.UserId, customerDetail.EMAILBalance, customerDetail.SMSBalance, customerDetail.NOTIFICATIONBalance);
                }

            });
            // Save status
            UpdateFiredAlerts(this.ProcessorId);
        }


        private List<ATDevice> GetFiredAlerts(int ProcessorId)
        {
            // TODO have to point to DAT table to process each & every record updated, instead of processing the current record
            List<ATDevice> devices = new List<ATDevice>();
            try
            {

                DataTable dt = new DataTable();
                dt = Data.GetData(DataBase.Alert, CommandType.StoredProcedure, "A_GetFiredAlerts", new SqlParameter[] { new SqlParameter("ProcessorId", ProcessorId) });

                devices = dt.AsEnumerable().GroupBy(dG => dG["DeviceId"]).Select(row =>
                    new ATDevice
                    {
                        Id = row.Key.ToString(),
                        Alerts = row.Select(dRow => new ATAlert()
                        {
                            Id = Convert.ToInt32(dRow["DeviceAlertId"]),
                            Name = Convert.ToString(dRow["Name"]),
                            DescriptionText = Convert.ToString(dRow["DescriptionText"]),
                            //IsSent = Convert.ToBoolean(dRow["IsSent"]),
                            //SentTime = Convert.ToDateTime(dRow["SentTime"]),
                            AlarmType = (Tracker.Common.Model.DeviceAlarmType)dRow["AlertType"],
                            Eval = Convert.ToString(dRow["Eval"]),
                            Conditions = AlertData.DeSerializeCondition(Convert.ToString(dRow["Eval"])),
                            //ConditionState = Convert.ToBoolean(dRow["ConditionState"]),
                            //ConditionStateTime = Convert.ToDateTime(dRow["ConditionStateTime"]),
                            CurrentData = new ATData()
                            {
                                VariableNVales = (new KeyValuePair<string, string>[] {
                                 new KeyValuePair<string, string>("CommandType", Convert.ToString(dRow["CommandType"])),
                                 new KeyValuePair<string, string>("StatusCode", Convert.ToString(dRow["StatusCode"])),
                                 new KeyValuePair<string, string>("Latitude", Convert.ToString(dRow["Latitude"])),
                                 new KeyValuePair<string, string>("Longitude", Convert.ToString(dRow["Longitude"])),
                                 new KeyValuePair<string, string>("Speed", Convert.ToString(dRow["Speed"])),
                                 new KeyValuePair<string, string>("Direction", Convert.ToString(dRow["Direction"])),
                                 new KeyValuePair<string, string>("OnBattery", Convert.ToString(dRow["OnBattery"])),
                                 new KeyValuePair<string, string>("OnIgnition", Convert.ToString(dRow["OnIgnition"])),
                                 new KeyValuePair<string, string>("OnAc", Convert.ToString(dRow["OnAc"])),
                                 new KeyValuePair<string, string>("OnGps", Convert.ToString(dRow["OnGps"])),
                                 new KeyValuePair<string, string>("OnAcc", Convert.ToString(dRow["OnAcc"])),
                                 new KeyValuePair<string, string>("OilNElectricConected", Convert.ToString(dRow["OilNElectricConected"])),
                                 new KeyValuePair<string, string>("OnSOS", Convert.ToString(dRow["OnSOS"])),
                                 new KeyValuePair<string, string>("OnLowBattery", Convert.ToString(dRow["OnLowBattery"])),
                                 new KeyValuePair<string, string>("OnPowerCut", Convert.ToString(dRow["OnPowerCut"])),
                                 new KeyValuePair<string, string>("OnShock", Convert.ToString(dRow["OnShock"])),
                                 new KeyValuePair<string, string>("OnCharge", Convert.ToString(dRow["OnCharge"])),
                                 new KeyValuePair<string, string>("OnDefence", Convert.ToString(dRow["OnDefence"])),
                                 new KeyValuePair<string, string>("AlarmType", Convert.ToString(dRow["AlarmType"])),
                                 new KeyValuePair<string, string>("DeviceDataTime", Convert.ToString(dRow["DeviceDataTime"])),
                                 new KeyValuePair<string, string>("TrackerConnectedTime", Convert.ToString(dRow["TrackerConnectedTime"])),
                                 new KeyValuePair<string, string>("TrackerDataActionTime", Convert.ToString(dRow["TrackerDataActionTime"])),
                                 new KeyValuePair<string, string>("TrackerDataParsedTime", Convert.ToString(dRow["TrackerDataParsedTime"])),
                                 new KeyValuePair<string, string>("ActionTime", Convert.ToString(dRow["ActionTime"])),
                                    //new KeyValuePair<string, string>("Duration", DateTime.UtcNow.Subtract(Convert.ToDateTime(dRow["ConditionStateTime"])).TotalMinutes.ToString())
                                }).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                            },
                            PreviousData = new ATData()
                            {
                                //VariableNVales = (new KeyValuePair<string, string>[] {
                                //     new KeyValuePair<string, string>("CommandType", Convert.ToString(dRow["PrevCommandType"])),
                                //     new KeyValuePair<string, string>("StatusCode", Convert.ToString(dRow["PrevStatusCode"])),
                                //     new KeyValuePair<string, string>("Latitude", Convert.ToString(dRow["PrevLatitude"])),
                                //     new KeyValuePair<string, string>("Longitude", Convert.ToString(dRow["PrevLongitude"])),
                                //     new KeyValuePair<string, string>("Speed", Convert.ToString(dRow["PrevSpeed"])),
                                //     new KeyValuePair<string, string>("Direction", Convert.ToString(dRow["PrevDirection"])),
                                //     new KeyValuePair<string, string>("OnBattery", Convert.ToString(dRow["PrevOnBattery"])),
                                //     new KeyValuePair<string, string>("OnIgnition", Convert.ToString(dRow["PrevOnIgnition"])),
                                //     new KeyValuePair<string, string>("OnAc", Convert.ToString(dRow["PrevOnAc"])),
                                //     new KeyValuePair<string, string>("OnGps", Convert.ToString(dRow["PrevOnGps"])),
                                //     new KeyValuePair<string, string>("OnAcc", Convert.ToString(dRow["PrevOnAcc"])),
                                //     new KeyValuePair<string, string>("OilNElectricConected", Convert.ToString(dRow["PrevOilNElectricConected"])),
                                //     new KeyValuePair<string, string>("OnSOS", Convert.ToString(dRow["PrevOnSOS"])),
                                //     new KeyValuePair<string, string>("OnLowBattery", Convert.ToString(dRow["PrevOnLowBattery"])),
                                //     new KeyValuePair<string, string>("OnPowerCut", Convert.ToString(dRow["PrevOnPowerCut"])),
                                //     new KeyValuePair<string, string>("OnShock", Convert.ToString(dRow["PrevOnShock"])),
                                //     new KeyValuePair<string, string>("OnCharge", Convert.ToString(dRow["PrevOnCharge"])),
                                //     new KeyValuePair<string, string>("OnDefence", Convert.ToString(dRow["PrevOnDefence"])),
                                //     new KeyValuePair<string, string>("AlarmType", Convert.ToString(dRow["PrevAlarmType"])),
                                //     new KeyValuePair<string, string>("DeviceDataTime", Convert.ToString(dRow["PrevDeviceDataTime"])),
                                //     new KeyValuePair<string, string>("TrackerConnectedTime", Convert.ToString(dRow["PrevTrackerConnectedTime"])),
                                //     new KeyValuePair<string, string>("TrackerDataActionTime", Convert.ToString(dRow["PrevTrackerDataActionTime"])),
                                //     new KeyValuePair<string, string>("TrackerDataParsedTime", Convert.ToString(dRow["PrevTrackerDataParsedTime"])),
                                //     new KeyValuePair<string, string>("ActionTime", Convert.ToString(dRow["PrevActionTime"]))
                                //}).ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                            }
                        }).ToList()

                    }
                ).ToList();
                dt = null;

            }
            catch (Exception)
            {
            }
            return devices;
        }

        private ATCustomerDetail GetCustomerDetails(string DeviceId)
        {
            ATCustomerDetail cDetail = new ATCustomerDetail();
            try
            {
                DataTable dt = new DataTable();
                dt = Data.GetData(DataBase.Alert, CommandType.StoredProcedure, "A_GetCustomerDetail",
                    new SqlParameter[] { new SqlParameter("DeviceId", DeviceId) });

                cDetail = dt.AsEnumerable().Select(row =>
                    new ATCustomerDetail()
                    {
                        UserId = Convert.ToString(row["UserId"]),
                        Username = Convert.ToString(row["Username"]),
                        FirstName = Convert.ToString(row["FirstName"]),
                        LastName = Convert.ToString(row["LastName"]),
                        Address_AddressLine1 = Convert.ToString(row["Address_AddressLine1"]),
                        Address_AddressLine2 = Convert.ToString(row["Address_AddressLine2"]),
                        Address_AddressLine3 = Convert.ToString(row["Address_AddressLine3"]),
                        Address_City = Convert.ToString(row["Address_City"]),
                        Address_State = Convert.ToString(row["Address_State"]),
                        Address_Country = Convert.ToString(row["Address_Country"]),
                        Address_PostalCode = Convert.ToString(row["Address_PostalCode"]),
                        PhoneNo = Convert.ToString(row["PhoneNo"]),
                        Email = Convert.ToString(row["Email"]),
                        WebSite = Convert.ToString(row["WebSite"]),
                        CompanyName = Convert.ToString(row["CompanyName"]),
                        Status = Convert.ToString(row["Status"]),
                        Parent = Convert.ToString(row["Parent"]),
                        Discriminator = Convert.ToString(row["Discriminator"]),

                        SMSBalance = Convert.ToInt32(row["SMSBalance"]),
                        EMAILBalance = Convert.ToInt32(row["EMAILBalance"]),
                        NOTIFICATIONBalance = Convert.ToInt32(row["NOTIFICATIONBalance"]),

                        IsAccountExpired = Convert.ToBoolean(row["IsAccountExpired"])
                    }
                ).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return cDetail;
        }

        private ATCustomerDetail SaveCustomerDetails(string UserId, int EMAILBalance, int SMSBalance, int NOTIFICATIONBalance)
        {
            ATCustomerDetail cDetail = new ATCustomerDetail();
            try
            {
                Data.StoreData_ExecuteNonQuery(DataBase.Alert, CommandType.StoredProcedure, "A_StoreCustomerNotificationCount",
                    new SqlParameter[] {
                        new SqlParameter("UserId", UserId),
                        new SqlParameter("SMSBalance", SMSBalance),
                        new SqlParameter("EMAILBalance", EMAILBalance),
                        new SqlParameter("NOTIFICATIONBalance", NOTIFICATIONBalance)
                    });
            }
            catch (Exception ex)
            {

            }
            return cDetail;
        }

        private List<AlertDeliveryNStatus> GetFiredAlertsReceivers(int DeviceAlertId)
        {
            // TODO have to point to DAT table to process each & every record updated, instead of processing the current record
            List<AlertDeliveryNStatus> deviceAlertReceivers = new List<AlertDeliveryNStatus>();

            DataTable dt = new DataTable();
            dt = Data.GetData(DataBase.Alert, CommandType.StoredProcedure, "A_GetFiredAlertReceivers",
                new SqlParameter[] { new SqlParameter("DeviceAlertId", DeviceAlertId) });

            deviceAlertReceivers = dt.AsEnumerable().Select(row =>
                new AlertDeliveryNStatus
                {
                    ToAddress = Convert.ToString(row["To"]),
                    MediumType = (AlertMediumType)Enum.Parse(typeof(AlertMediumType), Convert.ToString(row["MediumType"]))
                }
            ).ToList();

            dt = null;
            return deviceAlertReceivers;
        }

        private void StoreDeliverLog(string DeviceId, int DeviceAlertId, DeviceAlarmType AlertType, AlertDelivery ad)
        {
            try
            {
                ad.ToAddresses.ForEach(receivers =>
                {
                    Data.StoreData_ExecuteNonQuery(DataBase.Alert, CommandType.StoredProcedure, "A_StoreEmailLog",
                        new SqlParameter[] {
                             new SqlParameter("DeviceAlertId", DeviceAlertId),
                             new SqlParameter("DeviceId", DeviceId),
                             new SqlParameter("AlertType", AlertType),
                             new SqlParameter("Email", receivers.ToAddress),

                             new SqlParameter("EmailSubject", (ad.EmailContent != null) ? ad.EmailContent.Subject: string.Empty),
                             new SqlParameter("EmailPlainText", (ad.EmailContent != null) ? ad.EmailContent.PlainText: string.Empty),
                             new SqlParameter("EmailHtmlContent", (ad.EmailContent != null) ? ad.EmailContent.HtmlContent: string.Empty),
                             new SqlParameter("SmsSubject", (ad.SmsContent!= null) ?ad.SmsContent.Subject: string.Empty),
                             new SqlParameter("SmsPlainText", (ad.SmsContent!= null) ?ad.SmsContent.PlainText: string.Empty),

                             new SqlParameter("MediumType", Convert.ToInt32(receivers.MediumType)),
                             new SqlParameter("SentStatus", receivers.SentStatus.ToString()),
                             new SqlParameter("ErrorMessage", receivers.ErrorMessage)
                       });
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void UpdateFiredAlerts(int ProcessorId)
        {
            try
            {
                Data.StoreData_ExecuteNonQuery(DataBase.Alert, CommandType.StoredProcedure, "A_UpdateFiredAlerts",
                    new SqlParameter[] { new SqlParameter("ProcessorId", ProcessorId) });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #endregion

        #region Test Methods
        //[TestMethod]
        public void ProcessTestEmail()
        {

            AlertDelivery ed = new AlertDelivery();
            ed.EmailContent = TemplateParser.GetEmailTemplateWithValues(DeviceAlarmType.SpeedAlarm, new object { });

            ed.SmtpSettings = new SmtpSettings();
            ed.ToAddresses = new List<AlertDeliveryNStatus>();
            ed.ToAddresses.Add(new AlertDeliveryNStatus() { ToAddress = "iamkramesh@gmail.com" });
            ed.ToAddresses.Add(new AlertDeliveryNStatus() { ToAddress = "iamkrameshtest@gmail.com" });
            new EmailAlert().SendMail(ed);
        }

        public void TestSms()
        {

        }

        #endregion
    }
}