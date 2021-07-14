using Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Utility
{
    public static class Utils
    {
        static GLAccountBus glAccountBus = new GLAccountBus();
        static LoggedExpensesBus payableBus = new LoggedExpensesBus();
        static LoggedIncomeBus recievableBus = new LoggedIncomeBus();

        //Generate random password
        public static string GeneratePassword(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        //Generate random numbers
        private static Random rnd = new Random((int)DateTime.Now.Ticks);//thanks to McAden
        public static double GenerateNumber(int size)
        {
            double Start = Math.Pow(10, size - 1);
            double End = Math.Pow(10, (size));

            return (double)Math.Round(rnd.NextDouble() * (End - Start - 1)) + Start;
        }

        public static void SendMail(string to, string subject, string body)
        {

            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient("smtp.gmail.com");

            string From = ConfigurationManager.AppSettings["from"];
            string password = ConfigurationManager.AppSettings["password"];

            if (string.IsNullOrEmpty(to))
            {
                throw new Exception("No Destination Email");
            }

            mail.From = new MailAddress("otychinedu@gmail.com", "Core Bank Application", System.Text.Encoding.UTF8);
            mail.To.Add(to);
            mail.Subject = subject;
            //mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = body;
            //mail.BodyEncoding = System.Text.Encoding.UTF8;
            //mail.IsBodyHtml = true;
            //mail.Priority = MailPriority.High;

            client.Credentials = new System.Net.NetworkCredential(From, password);
            client.Port = 587;
            client.EnableSsl = true;

            bool sent = false;
            do
            {
                try
                {
                    client.Send(mail);
                    sent = true;
                    Console.WriteLine("Sent");
                }
                catch (Exception ex)
                {
                    sent = false;
                    Exception ex2 = ex;
                    string errorMessage = string.Empty;
                    while (ex2 != null)
                    {
                        errorMessage += ex2.ToString();
                        ex2 = ex2.InnerException;
                    }

                }
            } while (sent == false);
        }

        public static void SendMailTest(string to, string subject, string body)
        {

            MailMessage mail = new MailMessage();
            mail.To.Add(to);

            string From = ConfigurationManager.AppSettings["from"];
            string password = ConfigurationManager.AppSettings["password"];

            mail.From = new MailAddress("otychinedu@gmail.com", "Core Bank Application", System.Text.Encoding.UTF8);
            mail.Subject = subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = body;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("otychinedu@gmail.com", "germany97//");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            bool sent = false;
            do
            {
                try
                {
                    client.Send(mail);
                    sent = true;
                    Console.WriteLine("Sent");
                }
                catch (Exception ex)
                {
                    sent = false;
                    Exception ex2 = ex;
                    string errorMessage = string.Empty;
                    while (ex2 != null)
                    {
                        errorMessage += ex2.ToString();
                        ex2 = ex2.InnerException;
                    }

                }
            } while (sent == false);
        }
    }
}
