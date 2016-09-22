using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tracker.Common;
using Tracker.Common.Model;
using Utils;

namespace Alertify
{
    public static class ConditionProcessor
    {
        public static bool ProcessConditionWithOutDuration(ATAlert alert)
        {
            alert = SubstituteOperandWCValues(alert);
            return Eval(alert.Conditions.Where(m => m.Operand.ToLower() != "Duration".ToLower()).ToList());
        }

        public static bool ProcessDurationOnCondition(ATAlert alert)
        {
            alert = SubstituteDurationOperandWCValues(alert);
            return Eval(alert.Conditions);
        }

        public static bool ProcessFenceConditionWithOutDuration(ATAlert alert)
        {
            var FencePositionLs = alert.FencePosition.Select(m => new GeoPoint() { X = Double.Parse(m.Lat), Y = Double.Parse(m.Lang) }).ToList();

            if (alert.AlarmType == DeviceAlarmType.FenceInAlarm)
            {
                // Check inside
                return GeoPolygon.IsInside(FencePositionLs, Double.Parse(alert.CurrentData.VariableNVales["Latitude"]),
                    Double.Parse(alert.CurrentData.VariableNVales["Longitude"]));
            }
            else if (alert.AlarmType == DeviceAlarmType.FenceOutAlarm)
            {
                // Check outside
                return !GeoPolygon.IsInside(FencePositionLs, Double.Parse(alert.CurrentData.VariableNVales["Latitude"]),
                    Double.Parse(alert.CurrentData.VariableNVales["Longitude"]));
            }
            return true;
        }

        private static ATAlert SubstituteOperandWCValues(ATAlert alert)
        {
            for (int i = 0; i < alert.Conditions.Count; i++)
            {
                if (alert.Conditions[i].Operand.ToLower() != "Duration".ToLower())
                {
                    alert.Conditions[i].Operand = GetValuesFromData(alert.Conditions[i].Operand, alert.CurrentData);
                }
            }
            return alert;
        }

        private static ATAlert SubstituteDurationOperandWCValues(ATAlert alert)
        {
            for (int i = 0; i < alert.Conditions.Count; i++)
            {
                if (alert.Conditions[i].Operand.ToLower() == "Duration".ToLower())
                {
                    alert.Conditions[i].Operand = GetValuesFromData(alert.Conditions[i].Operand, alert.CurrentData);
                }
            }
            return alert;
        }

        private static string GetValuesFromData(string Operand, ATData currentData)
        {
            if (currentData.VariableNVales.ContainsKey(Operand))
            {
                return currentData.VariableNVales[Operand];
            }
            else
                return "0";
        }

        private static bool Eval(List<Condition> conditionWValues)
        {
            var thisResult = false;

            for (int i = 0; i < conditionWValues.Count; i++)
            {
                if (conditionWValues[i].Conjunction == ConjunctionType.FENCEBOTHINOUT ||
                    conditionWValues[i].Conjunction == ConjunctionType.FENCEIN ||
                    conditionWValues[i].Conjunction == ConjunctionType.FENCEOUT)
                {
                    // TODO
                    return true;
                }
                else
                {
                    thisResult = (EvalOperator(conditionWValues[i].Operator, Convert.ToDouble(conditionWValues[i].Operand), Convert.ToDouble(conditionWValues[i].Value)));
                    // On this point Operand will have CurrentData value

                    if (conditionWValues[i].Conjunction == ConjunctionType.AND)
                    {
                        if (thisResult == false)
                        {
                            return false;
                        }
                    }
                    else if (conditionWValues[i].Conjunction == ConjunctionType.OR)
                    {
                        if (thisResult == true)
                        {
                            return true;
                        }
                    }
                }
            }
            return thisResult;
        }

        private static bool EvalOperator(string Operator, double Operand1, double Operand2)
        {
            switch (Operator.Trim())
            {
                case "==":
                    if (Operand1 == Operand2)
                    {
                        return true;
                    }
                    break;
                case ">":
                    if (Operand1 > Operand2)
                    {
                        return true;
                    }
                    break;
                case ">=":
                    if (Operand1 >= Operand2)
                    {
                        return true;
                    }
                    break;
                case "<":
                    if (Operand1 < Operand2)
                    {
                        return true;
                    }
                    break;
                case "<=":
                    if (Operand1 <= Operand2)
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }


        // Not used
        public static bool IsPrevNCurrValuesChanged(ATAlert alert)
        {
            bool valueChanged = false;

            switch (alert.AlarmType)
            {
                case Tracker.Common.Model.DeviceAlarmType.NormalAlarm:
                    break;
                case Tracker.Common.Model.DeviceAlarmType.PowerCutAlarm:
                    break;
                case Tracker.Common.Model.DeviceAlarmType.SOSAlarm:
                    break;
                case Tracker.Common.Model.DeviceAlarmType.SpeedAlarm:
                    {
                        if (alert.CurrentData.VariableNVales["Speed"] != alert.PreviousData.VariableNVales["Speed"])
                        {
                            valueChanged = true;
                        }
                    }
                    break;
                case Tracker.Common.Model.DeviceAlarmType.BreakAlarm:
                    break;
                case Tracker.Common.Model.DeviceAlarmType.VibrationAlarm:
                    break;
                case Tracker.Common.Model.DeviceAlarmType.FenceAlarm:
                    break;
                case Tracker.Common.Model.DeviceAlarmType.MovingAlarm:
                    break;
                case Tracker.Common.Model.DeviceAlarmType.AccAlarm:
                    break;
                case Tracker.Common.Model.DeviceAlarmType.StopAlarm:
                    break;
                case Tracker.Common.Model.DeviceAlarmType.AcAlarm:
                    break;
                default:
                    break;
            }
            return valueChanged;
        }
    }
}