using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using Tracker.Common.Model;
using Utils;

namespace Alertify
{
    public class EmailAlert
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

        public static string Base_Host = System.Configuration.ConfigurationManager.AppSettings["SmtpClientHostAddress"];
        public static int Base_Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmtpClientHostPort"]);
        public static bool Base_EnableSsl = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SmtpClientEnableSsl"]);
        public static int Base_Timeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmtpClientTimeout"]);
        public static SmtpDeliveryMethod Base_DeliveryMethod = (SmtpDeliveryMethod)Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmtpClientDeliveryMethod"]);
        public static bool Base_UseDefaultCredentials = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["SmtpClientUseDefaultCredentials"]);
        public static string Base_CredentialUsername = System.Configuration.ConfigurationManager.AppSettings["SmtpClientCredentialUsername"];
        public static string Base_CredentialPassword = System.Configuration.ConfigurationManager.AppSettings["SmtpClientCredentialPassword"].ToString();
        public static string Base_CredentialUsernameDisplay = System.Configuration.ConfigurationManager.AppSettings["SmtpClientCredentialUsernameDisplay"];

        public AlertDelivery SendMail(AlertDelivery emailDelivery)
        {
            SmtpSettings smtpSettings = emailDelivery.SmtpSettings;

            if (smtpSettings == null)
            {
                smtpSettings = new SmtpSettings();
            }

            smtpSettings.Host = smtpSettings.Host ?? Base_Host;
            smtpSettings.Port = smtpSettings.Port ?? Base_Port;
            smtpSettings.EnableSsl = smtpSettings.EnableSsl ?? Base_EnableSsl;
            smtpSettings.Timeout = smtpSettings.Timeout ?? Base_Timeout;
            smtpSettings.DeliveryMethod = smtpSettings.DeliveryMethod ?? Base_DeliveryMethod;
            smtpSettings.UseDefaultCredentials = smtpSettings.UseDefaultCredentials ?? Base_UseDefaultCredentials;
            smtpSettings.CredentialUsername = smtpSettings.CredentialUsername ?? Base_CredentialUsername;
            smtpSettings.CredentialPassword = smtpSettings.CredentialPassword ?? Base_CredentialPassword;
            smtpSettings.CredentialUsernameDisplay = smtpSettings.CredentialUsernameDisplay ?? Base_CredentialUsernameDisplay;

            // Command line argument must the the SMTP host.
            SmtpClient client = new SmtpClient();

            client.Host = smtpSettings.Host;
            client.Port = smtpSettings.Port ?? default(int);
            client.EnableSsl = smtpSettings.EnableSsl ?? default(bool);
            client.Timeout = smtpSettings.Timeout ?? default(int);
            client.DeliveryMethod = smtpSettings.DeliveryMethod ?? default(SmtpDeliveryMethod);
            client.UseDefaultCredentials = smtpSettings.UseDefaultCredentials ?? default(bool);
            client.Credentials = new NetworkCredential(smtpSettings.CredentialUsername,
                smtpSettings.CredentialPassword);

            MailAddress from = new MailAddress(
                smtpSettings.CredentialUsername,
                smtpSettings.CredentialUsernameDisplay,
                Encoding.UTF8
               );

            emailDelivery.ToAddresses.ForEach(e =>
            {
                if (e.MediumType == AlertMediumType.Email)
                {
                    MailMessage message = new MailMessage();
                    message.From = from;
                    message.To.Add(new MailAddress(e.ToAddress));
                    message.Subject = emailDelivery.EmailContent.Subject;
                    message.Body = emailDelivery.EmailContent.PlainText;
                    AlternateView alternate = AlternateView.CreateAlternateViewFromString(emailDelivery.EmailContent.HtmlContent,
                        new ContentType("text/html"));
                    message.AlternateViews.Add(alternate);
                    try
                    {
                        client.Send(message);
                        Thread.Sleep(150);
                        e.SentStatus = DeliveryStatus.Success;
                    }
                    catch (Exception ex)
                    {
                        e.SentStatus = DeliveryStatus.Failed;
                        e.ErrorMessage = ex.Message + "\n" + (ex.InnerException == null ? "" : ex.InnerException.ToString());
                    }
                    finally
                    {
                        message = null;
                    }
                }
            });
            return emailDelivery;
        }
    }
}