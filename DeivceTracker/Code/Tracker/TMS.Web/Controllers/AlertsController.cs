using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TMS.Web.Models;
using TMS.Web.Models.ViewModels;
using Tracker.Common.Model;
using Utils;

namespace TMS.Web.Controllers
{
    public class AlertsController : Controller
    {
        #region Properties
        private static readonly string FileName = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType?.ToString();
        internal static Log4NetWrap Logger = new Log4NetWrap(FileName);
        #endregion

        public ActionResult Index()
        {
            return RedirectToAction("Schedule");
        }

        public ActionResult Schedule()
        {
            return View();
        }

        #region Ignition
        public ActionResult Ignition(int? Id = null)
        {
            IgnitionAlert alertModel = new IgnitionAlert();

            try
            {
                ScheduleViewModel sVM = new ScheduleViewModel();
                sVM.SelectedAlertType = DeviceAlarmType.AccAlarm;

                sVM = new AlertData().GetAlertDetails(sVM, Id);
                AlertBase alertData = sVM.AlertDatas.FirstOrDefault() as AlertBase;

                if (alertData == null)
                {
                    alertData = new AlertBase()
                    {
                        Id = 0,
                        Conditions = new List<Tracker.Common.Condition>(),
                        IsActive = false
                    };
                }

                bool IsOnAcc = false;

                if (alertData.Conditions.Where(m => m.Operand == "OnAcc").FirstOrDefault() != null)
                {
                    IsOnAcc = Convert.ToBoolean(Convert.ToInt32(alertData.Conditions.Where(m => m.Operand == "OnAcc").FirstOrDefault().Value));
                }
                else
                {
                    IsOnAcc = false;
                }

                alertModel = new IgnitionAlert()
                {
                    Id = alertData.Id,
                    IsActive = alertData.IsActive,
                    IsOnAcc = IsOnAcc,
                    Devices = new List<DeviceDetailSelection>()
                };
                List<DeviceDetailSelection> availableDevices = new AlertData().GetAvailableDevices();
                List<string> activeDevices = new AlertData().GetDevicesForAlert(alertData.Id);
                if (availableDevices != null && availableDevices.Count > 0)
                {
                    alertModel.Devices = availableDevices.Select(m => new DeviceDetailSelection
                    {
                        DeviceId = m.DeviceId,
                        VehicleId = m.VehicleId,
                        Checked = activeDevices.Contains(m.DeviceId)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return View(alertModel);
        }

        [HttpPost]
        public ActionResult Ignition(IgnitionAlert alertModel)
        {
            ScheduleViewModel sVM = new ScheduleViewModel();
            sVM.SelectedAlertType = DeviceAlarmType.AccAlarm;

            sVM.AlertDatas = new List<object>();
            AlertBase alertData = new AlertBase()
            {
                Id = alertModel.Id,
                IsActive = alertModel.IsActive,
                Conditions = new List<Tracker.Common.Condition>() {
                    new Tracker.Common.Condition() {
                        Operand = "OnAcc",
                        Value = alertModel.IsOnAcc == true ? "1" : "0"
                    }
                }
            };

            sVM.AlertDatas.Add(alertData);

            if (new AlertData().SaveAlertData(sVM))
            {
                int updateId = 0;
                if (int.TryParse(sVM.SelectedAlertId, out updateId))
                {
                    alertModel.Id = updateId;
                    if (alertModel.Devices != null && alertModel.Devices.Count > 0)
                    {
                        alertModel.Devices.ForEach(d =>
                        {
                            try
                            {
                                new AlertData().SaveDevicesForAlert(alertModel.Id, d.DeviceId, !d.Checked);
                            }
                            catch (Exception ex)
                            {
                            }
                        });
                    }
                }
                ViewBag.AlertWriteStatus = "Success";
            }
            else
            {
                ViewBag.AlertWriteStatus = "Failed";
            }

            return View(alertModel);
        }
        #endregion

        #region Speed
        public ActionResult Speed(int Id = 0)
        {
            SpeedAlert alertModel = new SpeedAlert();

            try
            {
                ScheduleViewModel sVM = new ScheduleViewModel();
                sVM.SelectedAlertType = DeviceAlarmType.SpeedAlarm;

                sVM = new AlertData().GetAlertDetails(sVM, Id);

                AlertBase alertData = sVM.AlertDatas.FirstOrDefault() as AlertBase;

                if (alertData == null)
                {
                    alertData = new AlertBase()
                    {
                        Id = 0,
                        Conditions = new List<Tracker.Common.Condition>(),
                        IsActive = false
                    };
                }

                int SpeedLimit = 50;
                string SpeedComparer = string.Empty;

                if (alertData.Conditions.Where(m => m.Operand == "Speed").FirstOrDefault() != null)
                {
                    SpeedLimit = Convert.ToInt32(alertData.Conditions.Where(m => m.Operand == "Speed").FirstOrDefault().Value);
                    SpeedComparer = Convert.ToString(alertData.Conditions.Where(m => m.Operand == "Speed").FirstOrDefault().Operator);
                }

                alertModel = new SpeedAlert()
                {
                    Id = alertData.Id,
                    Name = alertData.Name,
                    Description = alertData.DescriptionText,
                    IsActive = alertData.IsActive,
                    SpeedLimit = SpeedLimit,
                    SpeedComparer = SpeedComparer,
                    Devices = new List<DeviceDetailSelection>()
                };
                List<DeviceDetailSelection> availableDevices = new AlertData().GetAvailableDevices();
                List<string> activeDevices = new AlertData().GetDevicesForAlert(alertData.Id);
                if (availableDevices != null && availableDevices.Count > 0)
                {
                    alertModel.Devices = availableDevices.Select(m => new DeviceDetailSelection
                    {
                        DeviceId = m.DeviceId,
                        VehicleId = m.VehicleId,
                        Checked = activeDevices.Contains(m.DeviceId)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return View(alertModel);
        }

        [HttpPost]
        public ActionResult Speed(SpeedAlert alertModel)
        {
            {
                ScheduleViewModel sVM = new ScheduleViewModel();
                sVM.SelectedAlertType = DeviceAlarmType.SpeedAlarm;

                sVM.AlertDatas = new List<object>();
                AlertBase alertData = new AlertBase()
                {
                    Id = alertModel.Id,
                    Name = alertModel.Name,
                    DescriptionText = alertModel.Description,
                    IsActive = alertModel.IsActive,
                    Conditions = new List<Tracker.Common.Condition>() {
                        new Tracker.Common.Condition() {
                            Operand = "Speed",
                            Operator = alertModel.SpeedComparer,
                            Value = alertModel.SpeedLimit.ToString()
                        }
                    }
                };

                sVM.AlertDatas.Add(alertData);

                if (new AlertData().SaveAlertData(sVM))
                {
                    int updateId = 0;
                    if (int.TryParse(sVM.SelectedAlertId, out updateId))
                    {
                        alertModel.Id = updateId;
                        if (alertModel.Devices != null && alertModel.Devices.Count > 0)
                        {
                            alertModel.Devices.ForEach(d =>
                            {
                                try
                                {
                                    new AlertData().SaveDevicesForAlert(alertModel.Id, d.DeviceId, !d.Checked);
                                }
                                catch (Exception ex)
                                {
                                }
                            });
                        }
                    }
                    ViewBag.AlertWriteStatus = "Success";
                }
                else
                {
                    ViewBag.AlertWriteStatus = "Failed";
                }

                return View(alertModel);
            }
        }

        public ActionResult GetSpeedAlertList(string IdToShow)
        {
            var list = new AlertData().GetAvilableAlerts(DeviceAlarmType.SpeedAlarm);
            return View(list);
        }
        #endregion

        #region Stoppage
        public ActionResult Stoppage(int Id = 0)
        {
            StoppageAlert alertModel = new StoppageAlert();

            try
            {
                ScheduleViewModel sVM = new ScheduleViewModel();
                sVM.SelectedAlertType = DeviceAlarmType.StopAlarm;

                sVM = new AlertData().GetAlertDetails(sVM, Id);

                AlertBase alertData = sVM.AlertDatas.FirstOrDefault() as AlertBase;

                if (alertData == null)
                {
                    alertData = new AlertBase()
                    {
                        Id = 0,
                        Conditions = new List<Tracker.Common.Condition>(),
                        IsActive = false
                    };
                }

                int Duration = 5;
                string SpeedComparer = string.Empty;

                if (alertData.Conditions.Where(m => m.Operand == "Duration").FirstOrDefault() != null)
                {
                    Duration = Convert.ToInt32(alertData.Conditions.Where(m => m.Operand == "Duration").FirstOrDefault().Value);
                }

                alertModel = new StoppageAlert()
                {
                    Id = alertData.Id,
                    Name = alertData.Name,
                    Description = alertData.DescriptionText,
                    IsActive = alertData.IsActive,
                    Duration = Duration,
                    Devices = new List<DeviceDetailSelection>()
                };
                List<DeviceDetailSelection> availableDevices = new AlertData().GetAvailableDevices();
                List<string> activeDevices = new AlertData().GetDevicesForAlert(alertData.Id);
                if (availableDevices != null && availableDevices.Count > 0)
                {
                    alertModel.Devices = availableDevices.Select(m => new DeviceDetailSelection
                    {
                        DeviceId = m.DeviceId,
                        VehicleId = m.VehicleId,
                        Checked = activeDevices.Contains(m.DeviceId)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return View(alertModel);
        }

        [HttpPost]
        public ActionResult Stoppage(StoppageAlert alertModel)
        {
            {
                ScheduleViewModel sVM = new ScheduleViewModel();
                sVM.SelectedAlertType = DeviceAlarmType.StopAlarm;

                sVM.AlertDatas = new List<object>();
                AlertBase alertData = new AlertBase()
                {
                    Id = alertModel.Id,
                    Name = alertModel.Name,
                    DescriptionText = alertModel.Description,
                    IsActive = alertModel.IsActive,
                    Conditions = new List<Tracker.Common.Condition>() {
                        new Tracker.Common.Condition() {
                            Operand = "Speed",
                            Operator = "==",
                            Value = "0",
                            Conjunction = Tracker.Common.ConjunctionType.AND
                        },
                        new Tracker.Common.Condition() {
                            Operand = "Duration",
                            Operator = "<=",
                            Value = alertModel.Duration.ToString()
                        }
                    }
                };

                sVM.AlertDatas.Add(alertData);

                if (new AlertData().SaveAlertData(sVM))
                {
                    int updateId = 0;
                    if (int.TryParse(sVM.SelectedAlertId, out updateId))
                    {
                        alertModel.Id = updateId;
                        if (alertModel.Devices != null && alertModel.Devices.Count > 0)
                        {
                            alertModel.Devices.ForEach(d =>
                            {
                                try
                                {
                                    new AlertData().SaveDevicesForAlert(alertModel.Id, d.DeviceId, !d.Checked);
                                }
                                catch (Exception ex)
                                {
                                }
                            });
                        }
                    }
                    ViewBag.AlertWriteStatus = "Success";
                }
                else
                {
                    ViewBag.AlertWriteStatus = "Failed";
                }

                return View(alertModel);
            }
        }

        public ActionResult GetStoppageAlertList(string IdToShow)
        {
            var list = new AlertData().GetAvilableAlerts(DeviceAlarmType.StopAlarm);
            return View(list);
        }
        #endregion

        public ActionResult Moving()
        {
            MovingAlert alertModel = new MovingAlert()
            {
                Id = 0,
                IsActive = false
            };
            return View(alertModel);
        }

        #region Fence
        public ActionResult Fence(int Id = 0)
        {
            FenceAlert alertModel = new FenceAlert();

            try
            {
                ScheduleViewModel sVM = new ScheduleViewModel();
                sVM.SelectedAlertType = DeviceAlarmType.FenceAlarm;

                sVM = new AlertData().GetAlertDetails(sVM, Id);

                AlertBase alertData = sVM.AlertDatas.FirstOrDefault() as AlertBase;

                if (alertData == null)
                {
                    alertData = new AlertBase()
                    {
                        Id = 0,
                        Conditions = new List<Tracker.Common.Condition>(),
                        IsActive = false
                    };
                }

                alertModel = new FenceAlert()
                {
                    Id = alertData.Id,
                    Name = alertData.Name,
                    Description = alertData.DescriptionText,
                    IsActive = alertData.IsActive,
                    FenceType = (Id == 0) ? DeviceAlarmType.FenceInAlarm : sVM.SelectedAlertType,
                    Points = alertData.FenceList,
                    Devices = new List<DeviceDetailSelection>()
                };
                List<DeviceDetailSelection> availableDevices = new AlertData().GetAvailableDevices();
                List<string> activeDevices = new AlertData().GetDevicesForAlert(alertData.Id);
                if (availableDevices != null && availableDevices.Count > 0)
                {
                    alertModel.Devices = availableDevices.Select(m => new DeviceDetailSelection
                    {
                        DeviceId = m.DeviceId,
                        VehicleId = m.VehicleId,
                        Checked = activeDevices.Contains(m.DeviceId)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return View(alertModel);
        }

        [HttpPost]
        public ActionResult Fence(FenceAlert alertModel)
        {
            {
                ScheduleViewModel sVM = new ScheduleViewModel();
                //sVM.SelectedAlertType = DeviceAlarmType.FenceAlarm;
                sVM.SelectedAlertType = alertModel.FenceType;

                sVM.AlertDatas = new List<object>();
                AlertBase alertData = new AlertBase()
                {
                    Id = alertModel.Id,
                    Name = alertModel.Name,
                    DescriptionText = alertModel.Description,
                    IsActive = alertModel.IsActive,
                    FenceListStr = alertModel.PointsStr
                };

                sVM.AlertDatas.Add(alertData);

                if (new AlertData().SaveAlertData(sVM))
                {
                    int updateId = 0;
                    if (int.TryParse(sVM.SelectedAlertId, out updateId))
                    {
                        alertModel.Id = updateId;
                        try
                        {
                            alertModel.Points = (sVM.AlertDatas[0] as AlertBase).FenceList;
                        }
                        catch (Exception aBEx)
                        {
                        }

                        if (alertModel.Devices != null && alertModel.Devices.Count > 0)
                        {
                            alertModel.Devices.ForEach(d =>
                            {
                                try
                                {
                                    new AlertData().SaveDevicesForAlert(alertModel.Id, d.DeviceId, !d.Checked);
                                }
                                catch (Exception ex)
                                {
                                }
                            });
                        }
                    }
                    ViewBag.AlertWriteStatus = "Success";
                }
                else
                {
                    ViewBag.AlertWriteStatus = "Failed";
                }

                return View(alertModel);
            }
        }

        public ActionResult GetFenceAlertList(string IdToShow)
        {
            var list = new AlertData().GetAvilableAlerts(DeviceAlarmType.FenceAlarm);
            return View(list);
        }
        #endregion

        #region PowerCut
        public ActionResult PowerCut(int? Id = null)
        {
            PowerCutAlert alertModel = new PowerCutAlert();

            try
            {
                ScheduleViewModel sVM = new ScheduleViewModel();
                sVM.SelectedAlertType = DeviceAlarmType.PowerCutAlarm;

                sVM = new AlertData().GetAlertDetails(sVM, Id);
                AlertBase alertData = sVM.AlertDatas.FirstOrDefault() as AlertBase;

                if (alertData == null)
                {
                    alertData = new AlertBase()
                    {
                        Id = 0,
                        Conditions = new List<Tracker.Common.Condition>(),
                        IsActive = false
                    };
                }

                bool IsOnPowerCut = false;

                if (alertData.Conditions.Where(m => m.Operand == "OnPowerCut").FirstOrDefault() != null)
                {
                    IsOnPowerCut = Convert.ToBoolean(Convert.ToInt32(alertData.Conditions.Where(m => m.Operand == "OnPowerCut").FirstOrDefault().Value));
                }
                else
                {
                    IsOnPowerCut = false;
                }

                alertModel = new PowerCutAlert()
                {
                    Id = alertData.Id,
                    IsActive = alertData.IsActive,
                    IsOnPowerCut = IsOnPowerCut,
                    Devices = new List<DeviceDetailSelection>()
                };
                List<DeviceDetailSelection> availableDevices = new AlertData().GetAvailableDevices();
                List<string> activeDevices = new AlertData().GetDevicesForAlert(alertData.Id);
                if (availableDevices != null && availableDevices.Count > 0)
                {
                    alertModel.Devices = availableDevices.Select(m => new DeviceDetailSelection
                    {
                        DeviceId = m.DeviceId,
                        VehicleId = m.VehicleId,
                        Checked = activeDevices.Contains(m.DeviceId)
                    }).ToList();
                }
                // TODO: Get from db/old service
            }
            catch (Exception ex)
            {
            }
            return View(alertModel);
        }

        [HttpPost]
        public ActionResult PowerCut(PowerCutAlert alertModel)
        {
            ScheduleViewModel sVM = new ScheduleViewModel();
            sVM.SelectedAlertType = DeviceAlarmType.PowerCutAlarm;

            sVM.AlertDatas = new List<object>();
            AlertBase alertData = new AlertBase()
            {
                Id = alertModel.Id,
                IsActive = alertModel.IsActive,
                Conditions = new List<Tracker.Common.Condition>() {
                    new Tracker.Common.Condition() {
                        Operand = "OnPowerCut",
                        Value = alertModel.IsOnPowerCut == true ? "1" : "0"
                    }
                }
            };

            sVM.AlertDatas.Add(alertData);

            if (new AlertData().SaveAlertData(sVM))
            {
                int updateId = 0;
                if (int.TryParse(sVM.SelectedAlertId, out updateId))
                {
                    alertModel.Id = updateId;
                    if (alertModel.Devices != null && alertModel.Devices.Count > 0)
                    {
                        alertModel.Devices.ForEach(d =>
                        {
                            try
                            {
                                new AlertData().SaveDevicesForAlert(alertModel.Id, d.DeviceId, !d.Checked);
                            }
                            catch (Exception ex)
                            {
                            }
                        });
                    }
                }
                ViewBag.AlertWriteStatus = "Success";
            }
            else
            {
                ViewBag.AlertWriteStatus = "Failed";
            }

            return View(alertModel);
        }
        #endregion

        //public ActionResult Sos()
        //{
        //    SOSAlert alertModel = new SOSAlert()
        //    {
        //        Id = 0,
        //        IsActive = false
        //    };
        //    return View(alertModel);
        //}

        #region Ac
        public ActionResult Ac(int? Id = null)
        {
            AcAlert alertModel = new AcAlert();

            try
            {
                ScheduleViewModel sVM = new ScheduleViewModel();
                sVM.SelectedAlertType = DeviceAlarmType.AcAlarm;

                sVM = new AlertData().GetAlertDetails(sVM, Id);
                AlertBase alertData = sVM.AlertDatas.FirstOrDefault() as AlertBase;

                if (alertData == null)
                {
                    alertData = new AlertBase()
                    {
                        Id = 0,
                        Conditions = new List<Tracker.Common.Condition>(),
                        IsActive = false
                    };
                }

                bool IsOnAc = false;

                if (alertData.Conditions.Where(m => m.Operand == "OnAc").FirstOrDefault() != null)
                {
                    IsOnAc = Convert.ToBoolean(Convert.ToInt32(alertData.Conditions.Where(m => m.Operand == "OnAc").FirstOrDefault().Value));
                }
                else
                {
                    IsOnAc = false;
                }

                alertModel = new AcAlert()
                {
                    Id = alertData.Id,
                    IsActive = alertData.IsActive,
                    IsOnAc = IsOnAc,
                    Devices = new List<DeviceDetailSelection>()
                };
                List<DeviceDetailSelection> availableDevices = new AlertData().GetAvailableDevices();
                List<string> activeDevices = new AlertData().GetDevicesForAlert(alertData.Id);
                if (availableDevices != null && availableDevices.Count > 0)
                {
                    alertModel.Devices = availableDevices.Select(m => new DeviceDetailSelection
                    {
                        DeviceId = m.DeviceId,
                        VehicleId = m.VehicleId,
                        Checked = activeDevices.Contains(m.DeviceId)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return View(alertModel);
        }

        [HttpPost]
        public ActionResult Ac(AcAlert alertModel)
        {
            ScheduleViewModel sVM = new ScheduleViewModel();
            sVM.SelectedAlertType = DeviceAlarmType.AcAlarm;

            sVM.AlertDatas = new List<object>();
            AlertBase alertData = new AlertBase()
            {
                Id = alertModel.Id,
                IsActive = alertModel.IsActive,
                Conditions = new List<Tracker.Common.Condition>() {
                    new Tracker.Common.Condition() {
                        Operand = "OnAc",
                        Value = alertModel.IsOnAc == true ? "1" : "0"
                    }
                }
            };

            sVM.AlertDatas.Add(alertData);

            if (new AlertData().SaveAlertData(sVM))
            {
                int updateId = 0;
                if (int.TryParse(sVM.SelectedAlertId, out updateId))
                {
                    alertModel.Id = updateId;
                    if (alertModel.Devices != null && alertModel.Devices.Count > 0)
                    {
                        alertModel.Devices.ForEach(d =>
                        {
                            try
                            {
                                new AlertData().SaveDevicesForAlert(alertModel.Id, d.DeviceId, !d.Checked);
                            }
                            catch (Exception ex)
                            {
                            }
                        });
                    }
                }
                ViewBag.AlertWriteStatus = "Success";
            }
            else
            {
                ViewBag.AlertWriteStatus = "Failed";
            }

            return View(alertModel);
        }
        #endregion

        public ActionResult Delete(int AlertId, DeviceAlarmType alertType)
        {
            // Delete Alerts


            switch (alertType)
            {
                case DeviceAlarmType.NormalAlarm:
                    break;
                case DeviceAlarmType.PowerCutAlarm:
                    break;
                case DeviceAlarmType.SOSAlarm:
                    break;
                case DeviceAlarmType.SpeedAlarm:
                    break;
                case DeviceAlarmType.BreakAlarm:
                    break;
                case DeviceAlarmType.VibrationAlarm:
                    break;
                case DeviceAlarmType.FenceAlarm:
                    break;
                case DeviceAlarmType.MovingAlarm:
                    break;
                case DeviceAlarmType.AccAlarm:
                    break;
                case DeviceAlarmType.StopAlarm:
                    break;
                case DeviceAlarmType.AcAlarm:
                    break;
                case DeviceAlarmType.FenceOutAlarm:
                    break;
                case DeviceAlarmType.FenceInAlarm:
                    break;
                default:
                    break;
            }
            return Speed(0);
        }

        #region AlertReceivers
        public ActionResult AlertReceivers(string AlertId)
        {
            List<AlertReceiver> AlertReceivers = new List<AlertReceiver>();
            AlertReceivers = new AlertData().GetAlertReceivers(AlertId);
            return View(AlertReceivers);
        }

        [HttpPost]
        public ActionResult AlertReceivers(List<AlertReceiver> AlertReceivers, string AlertId)
        {
            new AlertData().SaveAlertReceivers(AlertReceivers, AlertId);
            return RedirectToAction("AlertReceivers", new { AlertId = AlertId });
        }
        #endregion
    }
}