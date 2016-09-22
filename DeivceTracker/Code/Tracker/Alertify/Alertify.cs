using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace Alertify
{
    public class Alertify
    {
        #region Properties
        private static readonly string _fileNm = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString();
        internal static Log4NetWrap log = new Log4NetWrap(_fileNm);
        #endregion

        #region Mail Configuration
        internal static readonly MailAddress FromMail;
        #endregion


        static Alertify()
        {
            string fromEmail = ConfigurationManager.AppSettings["FromEmail"];

            if (string.IsNullOrEmpty(fromEmail))
            {
                fromEmail = "madankumar6@gmail.com";
            }

            FromMail = new MailAddress(fromEmail, "Tracker Alert", System.Text.Encoding.UTF8);
        }

        public Alertify()
        {
        }
    }
}
