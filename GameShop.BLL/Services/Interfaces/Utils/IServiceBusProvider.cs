using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Services.Interfaces.Utils
{
    public interface IServiceBusProvider
    {
        Task SendMessageAsync(string msg);
    }
}
