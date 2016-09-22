using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using TMS.Web.Models.ViewModels;
//using Tracker.Common;
using Tracker.Common.Model;
using System.Linq;
using _DAL = DAL;
using TMS.DAL;
using Tracker.Common;
using System.Web.Mvc;

namespace TMS.Web.Models
{
    public class AlertData
    {
        public List<DeviceAlarmType> GetAvailableAlerts()
        {
            List<DeviceAlarmType> datL = new List<DeviceAlarmType>();

            datL.Add(DeviceAlarmType.PowerCutAlarm);

            datL.Add(DeviceAlarmType.SOSAlarm);
            datL.Add(DeviceAlarmType.SpeedAlarm);
            datL.Add(DeviceAlarmType.BreakAlarm);
            datL.Add(DeviceAlarmType.VibrationAlarm);
            datL.Add(DeviceAlarmType.FenceAlarm);
            datL.Add(DeviceAlarmType.FenceInAlarm);
            datL.Add(DeviceAlarmType.FenceOutAlarm);
            datL.Add(DeviceAlarmType.MovingAlarm);
            datL.Add(DeviceAlarmType.AccAlarm);
            datL.Add(DeviceAlarmType.StopAlarm);

            return datL;
        }

        public List<DeviceDetailSelection> GetAvailableDevices()
        {
            List<DeviceDetailSelection> devices = new List<DeviceDetailSelection>();

            try
            {
                DataTable dt = _DAL.Data.GetData(_DAL.DataBase.TcpServer, System.Data.CommandType.StoredProcedure, "Master_GetActiveDevices", null);
                if (dt != null || dt.Rows.Count > 0)
                {
                    devices = dt.AsEnumerable().Select(m =>
                        new DeviceDetailSelection()
                        {
                            DeviceId = Convert.ToString(m["DeviceId"]),
                            VehicleId = Convert.ToString(m["VehicleId"])
                        }
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
            }

            return devices;
        }

        public List<string> GetDevicesForAlert(int AlertId)
        {
            List<string> devices = new List<string>();

            try
            {
                DataTable dt = _DAL.Data.GetData(_DAL.DataBase.TcpServer, System.Data.CommandType.StoredProcedure, "Master_GetDevicesForAlert", new List<SqlParameter>() { new SqlParameter("DeviceAlertId", AlertId) }.ToArray());
                if (dt != null || dt.Rows.Count > 0)
                {
                    devices = dt.AsEnumerable().Select(d => Convert.ToString(d["DeviceId"])).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return devices;
        }

        public bool SaveDevicesForAlert(int AlertId, string DeviceId, bool IsToDelete)
        {
            List<string> devices = new List<string>();

            try
            {
                _DAL.Data.StoreData_ExecuteNonQuery(_DAL.DataBase.TcpServer, System.Data.CommandType.StoredProcedure, "Master_SaveDevicesForAlert", new List<SqlParameter>() {
                    new SqlParameter("DeviceAlertId", AlertId),
                    new SqlParameter("DeviceId", DeviceId),
                    new SqlParameter("IsToDelete", IsToDelete)
                }.ToArray());

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public List<AlertReceiver> GetAlertReceivers(string AlertId)
        {
            List<AlertReceiver> AlertReceivers = new List<AlertReceiver>();
            try
            {
                var alertsRcvrsDt = Data.GetData(DataBase.Api,
                    System.Data.CommandType.StoredProcedure, "Master_GetDeviceAlertReceivers", new List<SqlParameter>() { new SqlParameter("DeviceAlertId", AlertId) }.ToArray());
                if (alertsRcvrsDt != null)
                {
                    AlertReceivers = new List<AlertReceiver>();
                    foreach (var rcvr in alertsRcvrsDt.AsEnumerable())
                    {
                        AlertReceivers.Add(new AlertReceiver()
                        {
                            Id = Convert.ToInt32(rcvr["Id"] != DBNull.Value ? rcvr["Id"] : 0),
                            AlertId = AlertId,
                            To = Convert.ToString(rcvr["To"] != DBNull.Value ? rcvr["To"] : ""),
                            MediumType = (AlertMediumType)Convert.ToInt32(rcvr["MediumType"] != DBNull.Value ? rcvr["MediumType"] : 0)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return AlertReceivers;
        }

        public void SaveAlertReceivers(List<AlertReceiver> AlertRcvrs, string AlertId)
        {
            if (AlertRcvrs == null)
                return;

            List<SqlParameter[]> paramsToExe = new List<SqlParameter[]>();
            foreach (var aRcv in AlertRcvrs)
            {
                List<SqlParameter> newdbParams = new List<SqlParameter>();
                newdbParams.Add(new SqlParameter("DeviceAlertId", AlertId));
                newdbParams.Add(new SqlParameter("To", aRcv.To));
                newdbParams.Add(new SqlParameter("MediumType", aRcv.MediumType));
                newdbParams.Add(new SqlParameter("Id", aRcv.Id));
                newdbParams.Add(new SqlParameter("IsToDelete", aRcv.IsToDelete));
                paramsToExe.Add(newdbParams.ToArray());
            }

            try
            {
                Data.StoreData_ExecuteNonQuery_ParamList(DataBase.Api,
                    System.Data.CommandType.StoredProcedure, "Master_SaveDeviceAlertReceivers", paramsToExe);
            }
            catch (Exception ex)
            {
            }
        }


        public ScheduleViewModel GetAlertData(ScheduleViewModel svm)
        {
            DataTable alertsDt = new DataTable();
            try
            {
                List<SqlParameter> dbParams = new List<SqlParameter>();
                dbParams.Add(new SqlParameter("DeviceId", svm.SelectedDevice));
                dbParams.Add(new SqlParameter("AlertType", svm.SelectedAlertType));

                alertsDt = Data.GetData(DataBase.Api,
                    System.Data.CommandType.StoredProcedure, "Master_GetDeviceAlert", dbParams.ToArray());
            }
            catch (Exception ex)
            {
            }

            if (alertsDt != null)
            {
                switch (svm.SelectedAlertType)
                {
                    case DeviceAlarmType.PowerCutAlarm:
                        {
                            var alertDatas = new List<object>();

                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                alertDatas.Add(new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),
                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                });
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }
                    case DeviceAlarmType.SOSAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                alertDatas.Add(new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),
                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                });
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }
                    case DeviceAlarmType.SpeedAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                alertDatas.Add(new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),
                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                });
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }

                    //case DeviceAlarmType.BreakAlarm:
                    //    _AlertData = new BreakAlert();
                    //    break;
                    case DeviceAlarmType.VibrationAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                alertDatas.Add(new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),
                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                });
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }
                    case DeviceAlarmType.FenceAlarm:
                    case DeviceAlarmType.FenceInAlarm:
                    case DeviceAlarmType.FenceOutAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                var FenceList = new List<ATFPosition>();
                                try
                                {
                                    var fenceListDt = Data.GetData(DataBase.Api,
                                        System.Data.CommandType.StoredProcedure, "Master_GetFenceList",
                                        new SqlParameter[] {
                                            new SqlParameter("DeviceAlertId", aDt["Id"])
                                        });

                                    FenceList = fenceListDt.AsEnumerable().Select(m => new ATFPosition()
                                    {
                                        Lat = Convert.ToString(m["Latitude"]),
                                        Lang = Convert.ToString(m["Longitude"]),
                                        Distance = (float)Convert.ToDouble(m["Distance"]),
                                        ListOrder = Convert.ToInt32(m["ListOrder"])
                                    }).ToList();
                                }
                                catch (Exception ex)
                                {

                                }

                                alertDatas.Add(new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),

                                    Name = Convert.ToString(aDt["Name"]),
                                    DescriptionText = Convert.ToString(aDt["DescriptionText"]),

                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false),

                                    FenceList = FenceList ?? new List<ATFPosition>()
                                });
                            }
                            svm.AlertDatas = alertDatas;
                        }
                        break;
                    case DeviceAlarmType.MovingAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                var _aData = new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),
                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                };
                                _aData.Conditions.RemoveAll(a => a.Operand.ToLower() == "onacc");
                                alertDatas.Add(_aData);
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }
                    case DeviceAlarmType.AccAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                alertDatas.Add(new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),
                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                });
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }
                    case DeviceAlarmType.StopAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                var _aData = new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),
                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                };
                                alertDatas.Add(_aData);
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }
                    default:
                        break;
                }
            }
            return svm;
        }

        public ScheduleViewModel GetAlertDetails(ScheduleViewModel svm, int? DeviceAlertId)
        {
            DataTable alertsDt = new DataTable();
            try
            {
                List<SqlParameter> dbParams = new List<SqlParameter>();
                dbParams.Add(new SqlParameter("DeviceAlertId", DeviceAlertId));
                if (svm.SelectedAlertType != DeviceAlarmType.FenceAlarm)
                {
                    dbParams.Add(new SqlParameter("AlertType", svm.SelectedAlertType));
                }

                alertsDt = Data.GetData(DataBase.Api,
                    System.Data.CommandType.StoredProcedure, "Master_GetDeviceAlert", dbParams.ToArray());
            }
            catch (Exception ex)
            {
            }

            if (alertsDt != null)
            {
                switch (svm.SelectedAlertType)
                {
                    case DeviceAlarmType.PowerCutAlarm:
                        {
                            var alertDatas = new List<object>();

                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                alertDatas.Add(new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),

                                    Name = Convert.ToString(aDt["Name"]),
                                    DescriptionText = Convert.ToString(aDt["DescriptionText"]),

                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                });
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }
                    case DeviceAlarmType.SOSAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                alertDatas.Add(new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),

                                    Name = Convert.ToString(aDt["Name"]),
                                    DescriptionText = Convert.ToString(aDt["DescriptionText"]),

                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                });
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }
                    case DeviceAlarmType.SpeedAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                alertDatas.Add(new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),

                                    Name = Convert.ToString(aDt["Name"]),
                                    DescriptionText = Convert.ToString(aDt["DescriptionText"]),

                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                });
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }

                    //case DeviceAlarmType.BreakAlarm:
                    //    _AlertData = new BreakAlert();
                    //    break;
                    case DeviceAlarmType.VibrationAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                alertDatas.Add(new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),

                                    Name = Convert.ToString(aDt["Name"]),
                                    DescriptionText = Convert.ToString(aDt["DescriptionText"]),

                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                });
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }
                    case DeviceAlarmType.FenceAlarm:
                    case DeviceAlarmType.FenceInAlarm:
                    case DeviceAlarmType.FenceOutAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                svm.SelectedAlertType = (DeviceAlarmType)Enum.Parse(typeof(DeviceAlarmType), Convert.ToString(aDt["AlertType"]));

                                var FenceList = new List<ATFPosition>();
                                try
                                {
                                    var fenceListDt = Data.GetData(DataBase.Api,
                                        System.Data.CommandType.StoredProcedure, "Master_GetFenceList",
                                        new SqlParameter[] {
                                            new SqlParameter("DeviceAlertId", aDt["Id"])
                                        });

                                    FenceList = fenceListDt.AsEnumerable().Select(m => new ATFPosition()
                                    {
                                        Lat = Convert.ToString(m["Latitude"]),
                                        Lang = Convert.ToString(m["Longitude"]),
                                        Distance = (float)Convert.ToDouble(m["Distance"]),
                                        ListOrder = Convert.ToInt32(m["ListOrder"])
                                    }).ToList();
                                }
                                catch (Exception ex)
                                {

                                }

                                alertDatas.Add(new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),

                                    Name = Convert.ToString(aDt["Name"]),
                                    DescriptionText = Convert.ToString(aDt["DescriptionText"]),

                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false),

                                    FenceList = FenceList ?? new List<ATFPosition>()
                                });
                            }
                            svm.AlertDatas = alertDatas;
                        }
                        break;
                    case DeviceAlarmType.MovingAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                var _aData = new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),

                                    Name = Convert.ToString(aDt["Name"]),
                                    DescriptionText = Convert.ToString(aDt["DescriptionText"]),

                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                };
                                _aData.Conditions.RemoveAll(a => a.Operand.ToLower() == "onacc");
                                alertDatas.Add(_aData);
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }
                    case DeviceAlarmType.AcAlarm:
                    case DeviceAlarmType.AccAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                alertDatas.Add(new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),

                                    Name = Convert.ToString(aDt["Name"]),
                                    DescriptionText = Convert.ToString(aDt["DescriptionText"]),

                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                });
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }
                    case DeviceAlarmType.StopAlarm:
                        {
                            var alertDatas = new List<object>();
                            foreach (var aDt in alertsDt.AsEnumerable())
                            {
                                var _aData = new AlertBase()
                                {
                                    Id = Convert.ToInt32((aDt["Id"] != DBNull.Value ? aDt["Id"] : 0)),

                                    Name = Convert.ToString(aDt["Name"]),
                                    DescriptionText = Convert.ToString(aDt["DescriptionText"]),

                                    Conditions = Tracker.Common.AlertData.DeSerializeCondition(Convert.ToString(aDt["Eval"] != DBNull.Value ? aDt["Eval"] : "")),
                                    IsActive = Convert.ToBoolean(aDt["IsActive"] != DBNull.Value ? aDt["IsActive"] : false)
                                };
                                alertDatas.Add(_aData);
                            }
                            svm.AlertDatas = alertDatas;
                            break;
                        }
                    default:
                        break;
                }
            }
            return svm;
        }


        public bool SaveAlertData(ScheduleViewModel model)
        {
            try
            {
                List<CustomSqlParameterGroup> paramsToExe = new List<CustomSqlParameterGroup>();

                List<SqlParameter> parentdbParams = new List<SqlParameter>();
                //parentdbParams.Add(new SqlParameter("DeviceId", model.SelectedDevice));
                parentdbParams.Add(new SqlParameter("AlertType", model.SelectedAlertType));
                //dbParams.Add(new SqlParameter("IsActive", model.IsActive));

                switch (model.SelectedAlertType)
                {
                    case DeviceAlarmType.PowerCutAlarm:
                        Utils.Extensions.ToObjList<AlertBase>(model.AlertDatas).ForEach(ad =>
                        {
                            List<SqlParameter> newdbParams = new List<SqlParameter>();
                            newdbParams.AddRange(parentdbParams.ToArray());
                            newdbParams.Add(new SqlParameter("DeviceAlertsId", ad.Id));
                            newdbParams.Add(new SqlParameter("Name", ad.Name));
                            newdbParams.Add(new SqlParameter("DescriptionText", ad.DescriptionText));
                            newdbParams.Add(new SqlParameter("IsActive", ad.IsActive));
                            newdbParams.Add(new SqlParameter("IsToDelete", ad.IsToDelete));

                            Condition con = ad.Conditions[0];
                            con.Operator = "==";
                            con.Operand = "OnPowerCut";
                            // TODO
                            //con.Value = Convert.ToBoolean(con.Value) ? "1" : "0";
                            newdbParams.Add(new SqlParameter("Eval", Tracker.Common.AlertData.SerializeCondition(con)));

                            paramsToExe.Add(new CustomSqlParameterGroup() { Parameters = newdbParams.ToArray() });
                        });
                        break;
                    case DeviceAlarmType.SOSAlarm:
                        parentdbParams.Add(new SqlParameter("Operand", "OnSOS"));
                        parentdbParams.Add(new SqlParameter("Operator", "=="));
                        Utils.Extensions.ToObjList<AlertBase>(model.AlertDatas).ForEach(ad =>
                        {
                            List<SqlParameter> newdbParams = new List<SqlParameter>();
                            newdbParams.AddRange(parentdbParams.ToArray());

                            Condition con = ad.Conditions[0];
                            con.Operator = "==";
                            con.Operand = "OnSOS";
                            // TODO
                            //con.Value = Convert.ToBoolean(con.Value) ? "1" : "0";

                            newdbParams.Add(new SqlParameter("Eval", Tracker.Common.AlertData.SerializeCondition(con)));
                            newdbParams.Add(new SqlParameter("DeviceAlertsId", ad.Id));
                            newdbParams.Add(new SqlParameter("Name", ad.Name));
                            newdbParams.Add(new SqlParameter("DescriptionText", ad.DescriptionText));
                            newdbParams.Add(new SqlParameter("IsActive", ad.IsActive));
                            newdbParams.Add(new SqlParameter("IsToDelete", ad.IsToDelete));

                            paramsToExe.Add(new CustomSqlParameterGroup() { Parameters = newdbParams.ToArray() });
                        });
                        break;
                    case DeviceAlarmType.SpeedAlarm:
                        Utils.Extensions.ToObjList<AlertBase>(model.AlertDatas).ForEach(ad =>
                        {
                            List<SqlParameter> newdbParams = new List<SqlParameter>();
                            newdbParams.AddRange(parentdbParams.ToArray());
                            Condition con = ad.Conditions[0];
                            con.Operand = "Speed";
                            // TODO
                            //con.Value = Convert.ToBoolean(con.Value) ? "1" : "0";

                            newdbParams.Add(new SqlParameter("Eval", Tracker.Common.AlertData.SerializeCondition(con)));
                            newdbParams.Add(new SqlParameter("DeviceAlertsId", ad.Id));
                            newdbParams.Add(new SqlParameter("Name", ad.Name));
                            newdbParams.Add(new SqlParameter("DescriptionText", ad.DescriptionText));
                            newdbParams.Add(new SqlParameter("IsActive", ad.IsActive));
                            newdbParams.Add(new SqlParameter("IsToDelete", ad.IsToDelete));

                            paramsToExe.Add(new CustomSqlParameterGroup() { Parameters = newdbParams.ToArray() });
                        });
                        break;
                    //case DeviceAlarmType.BreakAlarm:
                    //    AlertData = new BreakAlert();
                    //    break;
                    case DeviceAlarmType.VibrationAlarm:
                        Utils.Extensions.ToObjList<AlertBase>(model.AlertDatas).ForEach(ad =>
                        {
                            List<SqlParameter> newdbParams = new List<SqlParameter>();
                            newdbParams.AddRange(parentdbParams.ToArray());
                            Condition con = ad.Conditions[0];
                            con.Operator = "==";
                            con.Operand = "Vibration";
                            // TODO
                            //con.Value = Convert.ToBoolean(con.Value) ? "1" : "0";

                            newdbParams.Add(new SqlParameter("Eval", Tracker.Common.AlertData.SerializeCondition(con)));
                            newdbParams.Add(new SqlParameter("DeviceAlertsId", ad.Id));
                            newdbParams.Add(new SqlParameter("IsActive", ad.IsActive));
                            newdbParams.Add(new SqlParameter("Name", ad.Name));
                            newdbParams.Add(new SqlParameter("DescriptionText", ad.DescriptionText));
                            newdbParams.Add(new SqlParameter("IsToDelete", ad.IsToDelete));

                            paramsToExe.Add(new CustomSqlParameterGroup() { Parameters = newdbParams.ToArray() });
                        });
                        break;
                    case DeviceAlarmType.FenceAlarm:
                    case DeviceAlarmType.FenceInAlarm:
                    case DeviceAlarmType.FenceOutAlarm:
                        {
                            Utils.Extensions.ToObjList<AlertBase>(model.AlertDatas).ForEach(ad =>
                            {
                                CustomSqlParameterGroup pListToProcess = new CustomSqlParameterGroup();
                                List<SqlParameter> newdbParams = new List<SqlParameter>();
                                newdbParams.AddRange(parentdbParams.ToArray());
                                newdbParams.Add(new SqlParameter("DeviceAlertsId", ad.Id));
                                newdbParams.Add(new SqlParameter("IsActive", ad.IsActive));
                                newdbParams.Add(new SqlParameter("IsToDelete", ad.IsToDelete));
                                newdbParams.Add(new SqlParameter("Name", ad.Name));
                                newdbParams.Add(new SqlParameter("DescriptionText", ad.DescriptionText));

                                pListToProcess.Parameters = newdbParams.ToArray();

                                pListToProcess.ChildParameters = new CustomSqlParameterGroup();

                                List<TVParameter> newdbFenceParams = new List<TVParameter>();

                                int listOrder = 0;

                                DataTable fenceDbDt = new DataTable();
                                fenceDbDt.Columns.Add("Col1", typeof(string));
                                fenceDbDt.Columns.Add("Col2", typeof(string));
                                fenceDbDt.Columns.Add("Col3", typeof(string));
                                fenceDbDt.Columns.Add("Col4", typeof(string));
                                fenceDbDt.Columns.Add("Col5", typeof(string));
                                fenceDbDt.Columns.Add("Col6", typeof(string));
                                fenceDbDt.Columns.Add("Col7", typeof(string));

                                if (!string.IsNullOrWhiteSpace(ad.FenceListStr))
                                {
                                    ad.FenceList = ad.FenceList ?? new List<ATFPosition>();
                                    ad.FenceListStr.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(m =>
                                    {
                                        var latLang = m.Trim().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        var atP = new ATFPosition()
                                        {
                                            Lat = latLang[0],
                                            Lang = latLang[1],
                                            ListOrder = listOrder
                                        };
                                        fenceDbDt.Rows.Add(ad.Id.ToString(), latLang[0].Trim(), latLang[1].Trim(), null, listOrder.ToString(), null, null);

                                        ad.FenceList.Add(atP);
                                        listOrder++;
                                    });
                                }

                                newdbFenceParams.Add(new TVParameter()
                                {
                                    ParameterName = "@DeviceFenceList",
                                    ParameterValue = fenceDbDt,
                                    ParameterTypeName = "CustomDataType",
                                    SqlDbType = SqlDbType.Structured
                                });

                                pListToProcess.ChildParameters.TableValuedParameters = newdbFenceParams.ToArray();
                                pListToProcess.ChildParameters.Parameters = new SqlParameter[] {
                                    new SqlParameter("DeviceAlertId", ad.Id),
                                    new SqlParameter("IsToDelete", ad.IsToDelete)
                                };

                                newdbFenceParams = null;
                                paramsToExe.Add(pListToProcess);
                            });
                        }
                        break;
                    case DeviceAlarmType.MovingAlarm:
                        Utils.Extensions.ToObjList<AlertBase>(model.AlertDatas).ForEach(ad =>
                        {
                            List<SqlParameter> newdbParams = new List<SqlParameter>();
                            newdbParams.AddRange(parentdbParams.ToArray());

                            for (int i = 0; i < ad.Conditions.Count; i++)
                            {
                                ad.Conditions[i].Operand = "Speed";
                            }

                            ad.Conditions.Insert(0, new Condition()
                            {
                                Operand = "OnAcc",
                                Operator = "==",
                                Value = "0",
                                Conjunction = ConjunctionType.AND
                            });

                            newdbParams.Add(new SqlParameter("Eval", Tracker.Common.AlertData.SerializeCondition(ad.Conditions)));
                            newdbParams.Add(new SqlParameter("DeviceAlertsId", ad.Id));
                            newdbParams.Add(new SqlParameter("IsActive", ad.IsActive));
                            newdbParams.Add(new SqlParameter("Name", ad.Name));
                            newdbParams.Add(new SqlParameter("DescriptionText", ad.DescriptionText));
                            newdbParams.Add(new SqlParameter("IsToDelete", ad.IsToDelete));


                            paramsToExe.Add(new CustomSqlParameterGroup() { Parameters = newdbParams.ToArray() });

                        });
                        break;
                    case DeviceAlarmType.AccAlarm:
                        Utils.Extensions.ToObjList<AlertBase>(model.AlertDatas).ForEach(ad =>
                        {
                            List<SqlParameter> newdbParams = new List<SqlParameter>();
                            newdbParams.AddRange(parentdbParams.ToArray());
                            Condition con = ad.Conditions[0];
                            con.Operator = "==";
                            con.Operand = "OnAcc";
                            // TODO
                            //con.Value = Convert.ToBoolean(con.Value) ? "1" : "0";

                            newdbParams.Add(new SqlParameter("Eval", Tracker.Common.AlertData.SerializeCondition(con)));
                            newdbParams.Add(new SqlParameter("DeviceAlertsId", ad.Id));
                            newdbParams.Add(new SqlParameter("IsActive", ad.IsActive));
                            newdbParams.Add(new SqlParameter("Name", ad.Name));
                            newdbParams.Add(new SqlParameter("DescriptionText", ad.DescriptionText));
                            newdbParams.Add(new SqlParameter("IsToDelete", ad.IsToDelete));


                            paramsToExe.Add(new CustomSqlParameterGroup() { Parameters = newdbParams.ToArray() });
                        });
                        break;
                    case DeviceAlarmType.AcAlarm:
                        Utils.Extensions.ToObjList<AlertBase>(model.AlertDatas).ForEach(ad =>
                        {
                            List<SqlParameter> newdbParams = new List<SqlParameter>();
                            newdbParams.AddRange(parentdbParams.ToArray());
                            Condition con = ad.Conditions[0];
                            con.Operator = "==";
                            con.Operand = "OnAc";
                            // TODO
                            //con.Value = Convert.ToBoolean(con.Value) ? "1" : "0";

                            newdbParams.Add(new SqlParameter("Eval", Tracker.Common.AlertData.SerializeCondition(con)));
                            newdbParams.Add(new SqlParameter("DeviceAlertsId", ad.Id));
                            newdbParams.Add(new SqlParameter("IsActive", ad.IsActive));
                            newdbParams.Add(new SqlParameter("Name", ad.Name));
                            newdbParams.Add(new SqlParameter("DescriptionText", ad.DescriptionText));
                            newdbParams.Add(new SqlParameter("IsToDelete", ad.IsToDelete));

                            paramsToExe.Add(new CustomSqlParameterGroup() { Parameters = newdbParams.ToArray() });
                        });
                        break;
                    case DeviceAlarmType.StopAlarm:
                        Utils.Extensions.ToObjList<AlertBase>(model.AlertDatas).ForEach(ad =>
                        {
                            List<SqlParameter> newdbParams = new List<SqlParameter>();
                            newdbParams.AddRange(parentdbParams.ToArray());

                            // For security reason, delete hijacked entries
                            ad.Conditions.RemoveAll(c => c.Operand.ToLower() != "speed" && c.Operand.ToLower() != "onacc" && c.Operand.ToLower() != "duration");

                            //for (int i = 0; i < ad.Conditions.Count; i++)
                            //{
                            //    ad.Conditions[i].Operand = "Speed";
                            //    ad.Conditions[i].Operator = "==";
                            //    ad.Conditions[i].Value = "0";
                            //}

                            //ad.Conditions.Insert(0, new Condition()
                            //{
                            //    Operand = "OnAcc",
                            //    Operator = "==",
                            //    Value = "0",
                            //    Conjunction = ConjunctionType.AND
                            //});

                            newdbParams.Add(new SqlParameter("Eval", Tracker.Common.AlertData.SerializeCondition(ad.Conditions)));
                            newdbParams.Add(new SqlParameter("DeviceAlertsId", ad.Id));
                            newdbParams.Add(new SqlParameter("IsActive", ad.IsActive));
                            newdbParams.Add(new SqlParameter("Name", ad.Name));
                            newdbParams.Add(new SqlParameter("DescriptionText", ad.DescriptionText));
                            newdbParams.Add(new SqlParameter("IsToDelete", ad.IsToDelete));


                            paramsToExe.Add(new CustomSqlParameterGroup() { Parameters = newdbParams.ToArray() });

                        });
                        break;
                    default:
                        break;
                }

                try
                {
                    paramsToExe.ForEach(p =>
                    {
                        string updatedVal = Data.StoreData_ExecuteScalar(DataBase.Api,
                            CommandType.StoredProcedure, "Master_SaveDeviceAlert",
                            p.Parameters,
                            null
                            );

                        model.SelectedAlertId = updatedVal;

                        if (p.ChildParameters != null && p.ChildParameters.TableValuedParameters != null && p.ChildParameters.TableValuedParameters.Length > 0)
                        {
                            switch (model.SelectedAlertType)
                            {
                                case DeviceAlarmType.FenceAlarm:
                                case DeviceAlarmType.FenceInAlarm:
                                case DeviceAlarmType.FenceOutAlarm:
                                    {
                                        // -1 used for delete the records
                                        if (!string.IsNullOrWhiteSpace(updatedVal) && updatedVal.Trim() != "-1")
                                        {
                                            p.ChildParameters.Parameters[0].Value = updatedVal.Trim();
                                        }

                                        Data.StoreData_ExecuteNonQuery(DataBase.Api,
                                            CommandType.StoredProcedure, "Master_SaveFenceList",
                                            p.ChildParameters.Parameters,
                                            p.ChildParameters.TableValuedParameters.ToList()
                                        );
                                    }
                                    break;
                            }
                        }

                    });

                }
                catch (Exception ex)
                {
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<AlertDetail> GetAvilableAlerts(DeviceAlarmType AlertType)
        {
            List<AlertDetail> list = new List<AlertDetail>();
            try
            {
                List<SqlParameter> dbParams = new List<SqlParameter>();
                if (AlertType != DeviceAlarmType.FenceAlarm)
                {
                    dbParams.Add(new SqlParameter("AlertType", AlertType));
                }
                DataTable alertsDt = Data.GetData(DataBase.Api,
                    System.Data.CommandType.StoredProcedure, "Master_GetAvailableAlerts", dbParams.ToArray());
                if (alertsDt != null && alertsDt.Rows.Count > 0)
                {
                    alertsDt.AsEnumerable().ToList().ForEach(a =>
                    {
                        if (AlertType == DeviceAlarmType.FenceAlarm)
                        {
                            DeviceAlarmType DbAlertType = (DeviceAlarmType)Enum.Parse(typeof(DeviceAlarmType), Convert.ToString(a["AlertType"]));
                            if (DbAlertType == DeviceAlarmType.FenceInAlarm || DbAlertType == DeviceAlarmType.FenceOutAlarm)
                            {
                                list.Add(new AlertDetail()
                                {
                                    Id = Convert.ToInt32(a["Id"]),
                                    Name = Convert.ToString(a["Name"])
                                });
                            }
                            else
                            {
                                // Skip other than fence
                            }
                        }
                        else
                        {
                            list.Add(new AlertDetail()
                            {
                                Id = Convert.ToInt32(a["Id"]),
                                Name = Convert.ToString(a["Name"])
                            });
                        }


                    });
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }
    }
}