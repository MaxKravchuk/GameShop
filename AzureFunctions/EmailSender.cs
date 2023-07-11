using System;
using Microsoft.Azure.WebJobs;
using GameShop.BLL.Services.Interfaces.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;

namespace AzureFunctions
{
    public static class EmailSender
    {
        private const string EmailSub = "GameStore";
        private const string EmailBody = "Thx for registration";

        [FunctionName("EmailSender")]
        public static void Run([ServiceBusTrigger("regqueue", Connection = "ServiceBusConnectionString")]string customerEmail, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {customerEmail}");

            string senderEmail = Environment.GetEnvironmentVariable("SenderEmail");
            string senderPassword = Environment.GetEnvironmentVariable("SenderPassword");

            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(senderEmail);
                mailMessage.To.Add(customerEmail);
                mailMessage.Subject = EmailSub;
                mailMessage.Body = EmailBody;

                using (SmtpClient smtpClient = new SmtpClient())
                {
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;
                    try
                    {
                        smtpClient.Send(mailMessage);
                    }
                    catch(Exception x)
                    {
                        log.LogError(x.InnerException.Message);
                    }
                }
            }

            log.LogInformation($"Email sent to: {customerEmail}");
        }
    }
}
