using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.StrategyDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Strategies.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Strategies
{
    public class IBoxStrategy : IPaymentStrategy
    {
        public PaymentResultDTO Pay(Order newOrder)
        {
            var result = new PaymentResultDTO
            {
                OrderId = newOrder.Id
            };

            return result;
        }
    }
}
