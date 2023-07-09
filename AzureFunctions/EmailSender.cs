using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using GameShop.BLL.Services.Utils;
using GameShop.BLL.Services.Interfaces.Utils;

namespace AzureFunctions
{
    public static class EmailSender
    {
        [FunctionName("EmailSender")]
        public static void Run([ServiceBusTrigger("regqueue", Connection = "ServiceBusConnectionString")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
