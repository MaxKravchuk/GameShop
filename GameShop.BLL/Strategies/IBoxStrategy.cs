using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Strategies.Interfaces;

namespace GameShop.BLL.Strategies
{
    public class IBoxStrategy : IPaymentStrategy<int>
    {
        public async Task<int> Pay(OrderCreateDTO orderCreateDTO, IOrderService orderService)
        {
            var newOrder = await orderService.CreateOrderAsync(orderCreateDTO);
            return newOrder.Id;
        }
    }
}
