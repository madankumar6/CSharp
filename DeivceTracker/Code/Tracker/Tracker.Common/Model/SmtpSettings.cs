using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Tracker.Common.Model
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int? Port { get; set; }
        public bool? EnableSsl { get; set; }
        public int? Timeout { get; set; }
        public SmtpDeliveryMethod? DeliveryMethod { get; set; }
        public bool? UseDefaultCredentials { get; set; }
        public string CredentialUsernameDisplay { get; set; }
        public string CredentialUsername { get; set; }
        public string CredentialPassword { get; set; }
    }

    public class SMSApiSettings
    {
        public string ApiUrl { get; set; }
        public string ApiUrlWithCredientials { get; set; }
    }

    public class EmailContent
    {
        public string Subject { get; set; }
        public string PlainText { get; set; }
        public string HtmlContent { get; set; }
    }

    public class SmsContent
    {
        public string Subject { get; set; }
        public string PlainText { get; set; }
    }

    public enum DeliveryStatus {
        Success,
        Failed
    }

    public class AlertDeliveryNStatus {
        public string ToAddress { get; set; }
        public AlertMediumType MediumType { get; set; }
        public DeliveryStatus SentStatus { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AlertDelivery
    {
        public List<AlertDeliveryNStatus> ToAddresses { get; set; }
        public EmailContent EmailContent { get; set; }
        public SmsContent SmsContent { get; set; }
        public SmtpSettings SmtpSettings { get; set; }
        public SMSApiSettings SMSApiSettings { get; set; }
    }
}
