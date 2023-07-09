using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Services.Interfaces.Utils;
using Microsoft.Azure.ServiceBus;

namespace GameShop.BLL.Services.Utils
{
    public class ServiceBusProvider : IServiceBusProvider
    {
        private readonly QueueClient _queueClient;

        public ServiceBusProvider(QueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        public async Task SendMessageAsync(string msg)
        {
            var msgToSend = new Message(
                Encoding.UTF8.GetBytes(msg))
            {
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "application/json"
            };
            await _queueClient.SendAsync(msgToSend);
        }
    }
}
