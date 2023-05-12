using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Services.Interfaces;

namespace GameShop.BLL.Strategies.Interfaces
{
    public interface IPaymentContext<T>
    {
        Task<T> ExecuteStrategy(OrderCreateDTO orderCreateDTO, IOrderService orderService);
    }
}
