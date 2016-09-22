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

namespace Alertify
{
    public class ATDevice
    {
        public string Id;
        public List<ATAlert> Alerts;
    }

    public class ATAlert
    {
        public int Id;

        public string Name;
        public string DescriptionText;

        public bool IsSent;
        public DateTime SentTime;

        public DeviceAlarmType AlarmType;
        public List<Condition> Conditions;
        public ATData CurrentData;
        public ATData PreviousData;

        public string Eval { get; internal set; }

        public DateTime? ConditionStateTime { get; set; }
        public bool ConditionState { get; set; }

        public List<ATFPosition> FencePosition { get; set; }
    }

    public class ATData
    {
        public Dictionary<string, string> VariableNVales;
    }

    public class ATCustomerDetail
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address_AddressLine1 { get; set; }
        public string Address_AddressLine2 { get; set; }
        public string Address_AddressLine3 { get; set; }
        public string Address_City { get; set; }
        public string Address_State { get; set; }
        public string Address_Country { get; set; }
        public string Address_PostalCode { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string CompanyName { get; set; }
        public string Status { get; set; }
        public string Parent { get; set; }
        public string Discriminator { get; set; }

        public int SMSBalance { get; set; }
        public int EMAILBalance { get; set; }
        public int NOTIFICATIONBalance { get; set; }

        public bool IsAccountExpired { get; set; }
    }

}