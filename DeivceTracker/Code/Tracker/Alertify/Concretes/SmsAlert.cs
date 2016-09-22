using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading;
using Tracker.Common.Model;
using Utils;

namespace Alertify
{
    public class SmsAlert
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

        public static string SmsApiUrl = System.Configuration.ConfigurationManager.AppSettings["SmsApiUrl"];
        public static string SmsApiUrlWithCredientials = System.Configuration.ConfigurationManager.AppSettings["SmsUrlWithCredientials"];

        public AlertDelivery SendMessage(AlertDelivery alertDelivery)
        {
            //SmsUrlWithCredientials = "http://api.textlocal.in/send/?username=" + userName + "&apiKey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
            //string SmsUrlWithCredientials = "http://api.textlocal.in/send/?username=joinwithbalu@gmail.com&apiKey=vzCOfy3HGAc-NkjC02r1abi5iEz1tiqTYmkbvCpKM2&numbers={0}&message={1}&sender=TXTLCL";

            string _SmsApiUrl = alertDelivery.SMSApiSettings.ApiUrl ?? SmsApiUrl;
            string _SmsApiUrlWithCredientials = alertDelivery.SMSApiSettings.ApiUrlWithCredientials ?? SmsApiUrlWithCredientials;

            alertDelivery.ToAddresses.ForEach(e =>
            {
                if (e.MediumType == AlertMediumType.SMS)
                {

                    string _url = string.Format(_SmsApiUrl, "&", e.ToAddress, alertDelivery.SmsContent.PlainText, alertDelivery.SmsContent.Subject);
                    string _data = string.Format(_SmsApiUrlWithCredientials, "&", e.ToAddress, alertDelivery.SmsContent.PlainText, alertDelivery.SmsContent.Subject);

                    string result = SendSms(_url, _data);

                    if (!string.IsNullOrWhiteSpace(result))
                    {
                        // Not a c# issue
                        if (result.Contains("\"status\":\"success\""))
                        {
                            e.SentStatus = DeliveryStatus.Success;
                            e.ErrorMessage = result;
                        }
                        else if (result.Contains("\"status\":\"failure\""))
                        {
                            e.SentStatus = DeliveryStatus.Failed;
                            e.ErrorMessage = result;
                        }
                        else
                        {
                            // Technical issue
                            e.SentStatus = DeliveryStatus.Failed;
                            e.ErrorMessage = "Technical issue " + result;
                        }
                    }
                    else
                    {
                        e.SentStatus = DeliveryStatus.Failed;
                        e.ErrorMessage = result;
                    }

                }
            });

            return alertDelivery;
        }

        private string SendSms(string url, string urlWithData)
        {
            string result = string.Empty;

            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(urlWithData);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(urlWithData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                myWriter.Close();
            }

            try
            {
                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }
    }
}