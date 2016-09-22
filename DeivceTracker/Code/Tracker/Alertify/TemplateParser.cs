using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using Tracker.Common.Model;

namespace Alertify
{
    public static class TemplateParser
    {
        public static Dictionary<string, string> EmailSubjectTemplate { get; set; }
        public static Dictionary<string, string> EmailPlainTemplate { get; set; }
        public static Dictionary<string, string> EmailHtmlTemplate { get; set; }

        public static Dictionary<string, string> SmsSubjectTemplate { get; set; }
        public static Dictionary<string, string> SmsPlainTemplate { get; set; }
        static TemplateParser()
        {
            EmailSubjectTemplate = EmailSubjectTemplate ?? new Dictionary<string, string>();
            EmailPlainTemplate = EmailPlainTemplate ?? new Dictionary<string, string>();
            EmailHtmlTemplate = EmailHtmlTemplate ?? new Dictionary<string, string>();

            SmsSubjectTemplate = SmsSubjectTemplate ?? new Dictionary<string, string>();
            SmsPlainTemplate = SmsPlainTemplate ?? new Dictionary<string, string>();
        }

        public static EmailContent GetEmailTemplateWithValues(DeviceAlarmType deviceAlarmType, object Model)
        {
            EmailContent ec = new EmailContent();
            try
            {
                string FileNameToRead = string.Empty;

                string AlarmTypeStr = deviceAlarmType.ToString();

                string fileContent = string.Empty;

                if (!EmailSubjectTemplate.ContainsKey(AlarmTypeStr))
                {
                    fileContent = string.Empty;
                    fileContent = System.IO.File.ReadAllText(@"Templates\Email\" + AlarmTypeStr + @"Subject.txt");
                    EmailSubjectTemplate.Add(AlarmTypeStr, fileContent);
                }

                if (!EmailPlainTemplate.ContainsKey(AlarmTypeStr))
                {
                    fileContent = string.Empty;
                    fileContent = System.IO.File.ReadAllText(@"Templates\Email\" + AlarmTypeStr + @"Plain.txt");
                    EmailPlainTemplate.Add(AlarmTypeStr, fileContent);
                }

                if (!EmailHtmlTemplate.ContainsKey(AlarmTypeStr))
                {
                    fileContent = string.Empty;
                    fileContent = System.IO.File.ReadAllText(@"Templates\Email\" + AlarmTypeStr + @"Html.html");
                    EmailHtmlTemplate.Add(AlarmTypeStr, fileContent);
                }

                ec.Subject = EmailSubjectTemplate[AlarmTypeStr];
                ec.PlainText = EmailPlainTemplate[AlarmTypeStr];
                ec.HtmlContent = EmailHtmlTemplate[AlarmTypeStr];

                ec.Subject = Engine.Razor.RunCompile(ec.Subject, AlarmTypeStr + "Subject", null, Model);
                ec.PlainText = Engine.Razor.RunCompile(ec.PlainText, AlarmTypeStr + "Plain", null, Model);
                ec.HtmlContent = Engine.Razor.RunCompile(ec.HtmlContent, AlarmTypeStr + "Html", null, Model);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return ec;
        }

        public static SmsContent GetSmsTemplateWithValues(DeviceAlarmType deviceAlarmType, object Model)
        {
            SmsContent sc = new SmsContent();
            try
            {
                string FileNameToRead = string.Empty;

                string AlarmTypeStr = deviceAlarmType.ToString();

                string fileContent = string.Empty;

                if (!SmsSubjectTemplate.ContainsKey(AlarmTypeStr))
                {
                    fileContent = string.Empty;
                    fileContent = System.IO.File.ReadAllText(@"Templates\Sms\" + AlarmTypeStr + @"Subject.txt");
                    SmsSubjectTemplate.Add(AlarmTypeStr, fileContent);
                }

                if (!SmsPlainTemplate.ContainsKey(AlarmTypeStr))
                {
                    fileContent = string.Empty;
                    fileContent = System.IO.File.ReadAllText(@"Templates\Sms\" + AlarmTypeStr + @"Plain.txt");
                    SmsPlainTemplate.Add(AlarmTypeStr, fileContent);
                }

                sc.Subject = SmsSubjectTemplate[AlarmTypeStr];
                sc.PlainText = SmsPlainTemplate[AlarmTypeStr];

                sc.Subject = Engine.Razor.RunCompile(sc.Subject, AlarmTypeStr + "SmsSubject", null, Model);
                sc.PlainText = Engine.Razor.RunCompile(sc.PlainText, AlarmTypeStr + "SmsPlain", null, Model);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return sc;
        }
    }
}