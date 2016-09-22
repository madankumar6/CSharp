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
    public class Condition
    {
        public string Operand { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
        public ConjunctionType Conjunction { get; set; }
    }

    public enum ConjunctionType
    {
        NONE = 0,
        AND = 1,
        OR,

        FENCEIN = 10,
        FENCEOUT,
        FENCEBOTHINOUT
    }

    public static class AlertData
    {
        public static List<Condition> DeSerializeCondition(string EvalString)
        {
            List<Condition> cons = new List<Condition>();
            try
            {
                if (!string.IsNullOrWhiteSpace(EvalString))
                {
                    var typeSplits = new ConjunctionType[] {
                        ConjunctionType.AND,
                        ConjunctionType.OR,
                        ConjunctionType.FENCEIN,
                        ConjunctionType.FENCEOUT,
                        ConjunctionType.FENCEBOTHINOUT
                    };

                    foreach (var item in typeSplits)
                    {
                        EvalString = EvalString.Replace(item.ToString(), "<<<" + Convert.ToInt32(item).ToString() + ">>>" + item.ToString());
                    }

                    var strSplits = new string[] {
                        ConjunctionType.AND.ToString(),
                        ConjunctionType.OR.ToString(),
                        ConjunctionType.FENCEIN.ToString(),
                        ConjunctionType.FENCEOUT.ToString(),
                        ConjunctionType.FENCEBOTHINOUT.ToString()
                    };

                    //({Speed} >= [10]) AND ({Acc}) AND ({Acc})`
                    //({Speed} >= [10]) <0>AND ({Acc}) <0>AND ({Acc})`
                    //({Speed} >= [10]) <0>, ({Acc}) <0>, ({Acc})`
                    //({Speed} >= [10])

                    foreach (var spStr in EvalString.Split(strSplits, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var Conjunction = ConjunctionType.NONE;
                        int cTIndex = spStr.IndexOf("<<<");
                        if (cTIndex > -1)
                        {
                            Conjunction = (ConjunctionType)Convert.ToInt32(spStr.Substring(cTIndex + 3).Replace(">>>", string.Empty));
                        }
                        var spStrs = spStr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (spStrs.Length >= 3)
                        {
                            cons.Add(new Condition()
                            {
                                Conjunction = Conjunction,
                                Operand = spStrs[0],
                                Operator = spStrs[1],
                                Value = spStrs[2]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return cons;
        }

        public static string SerializeCondition(Condition Conditions)
        {
            return SerializeCondition(new List<Condition> {
                new Condition() {
                Conjunction = ConjunctionType.NONE,
                Operand = Conditions.Operand,
                Operator = Conditions.Operator,
                Value = Conditions.Value
                }
            });
        }

        public static string SerializeCondition(List<Condition> Conditions)
        {
            StringBuilder EvalString = new StringBuilder();
            try
            {
                foreach (var cond in Conditions)
                {
                    EvalString.Append(cond.Operand);
                    EvalString.Append(' ');
                    EvalString.Append(cond.Operator);
                    EvalString.Append(' ');
                    EvalString.Append(cond.Value);
                    if (cond.Conjunction != ConjunctionType.NONE)
                    {
                        EvalString.Append(' ');
                        EvalString.Append(cond.Conjunction.ToString());
                        EvalString.Append(' ');
                    }
                }
            }
            catch (Exception ex) { }
            return EvalString.ToString();
        }


    }
}