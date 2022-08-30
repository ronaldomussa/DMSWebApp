using System.Net.Mail;
using System.Text;
using System;
using System.Security.Cryptography;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ENI
{
    public class Util
    {
        public static bool SendEmail(string name, string emailTo, string subject, string message, string emailReplyTo)
        {
            try
            {
                MailMessage objMail = new MailMessage();

                // send by
                objMail.From = new MailAddress(Constants.SMTPSetup.EMAIL_FROM, Constants.SMTPSetup.EMAIL_FROM_NAME);

                // reply to
                if (string.IsNullOrEmpty(emailReplyTo))
                    objMail.ReplyToList.Add(objMail.From);
                else
                    objMail.ReplyToList.Add(new MailAddress(emailReplyTo));

                // send to
                if (string.IsNullOrEmpty(emailTo))
                    objMail.To.Add(objMail.From);
                else
                    objMail.To.Add(new MailAddress(emailTo, name));
                
                objMail.Headers.Add("Content-Type", "text/html");
                objMail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
                objMail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
                objMail.IsBodyHtml = true;

                objMail.Subject = subject;
                objMail.Body = message;

                SmtpClient objSMTP = new SmtpClient(Constants.SMTPSetup.SMTP, Constants.SMTPSetup.PORT);

                System.Net.NetworkCredential credencial = new System.Net.NetworkCredential(Constants.SMTPSetup.CREDENTIAL_EMAIL, Constants.SMTPSetup.CREDENTIAL_PASSWORD);

                objSMTP.UseDefaultCredentials = false;
                objSMTP.Credentials = credencial;

                //objSMTP.EnableSsl = true; 
                objSMTP.Send(objMail);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static string GenerateRandomNumber(int count)
        {
            Random rnd = new Random();
            string randomSequence = string.Empty;

            for (int i = 0; i < count; i++)
            {
                randomSequence += rnd.Next(0, 9).ToString();
            }

            return randomSequence;
        }

        public static bool IsValidEmail(string email)
        {
            string regex = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}$";

            var match = Regex.Match(email, regex, RegexOptions.IgnoreCase);

            return match.Success;
        }

        public static string stringTrunc(string text)
        {
            string cryptKey;
            Byte[] text_bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
            cryptKey = Convert.ToBase64String(text_bytes);
            return cryptKey;
        }

        public static string stringUntrunc(string text)
        {
            string UncryptKey;
            Byte[] text_bytes = Convert.FromBase64String(text);
            UncryptKey = System.Text.ASCIIEncoding.ASCII.GetString(text_bytes);
            return UncryptKey;
        }

        public static string GenerateHashMd5(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Converter a String para array de bytes, que é como a biblioteca trabalha.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Cria-se um StringBuilder para recompôr a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop para formatar cada byte como uma String em hexadecimal
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }

        public static decimal convertRealToDecimal(string real) {

            string real_converted = real
                .Replace("R$", "")
                .Replace(".", "")
                //.Replace(",", ".")
                .Trim();

            var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "," };

            return decimal.Parse(real_converted, numberFormatInfo);
        }
 
    }
}