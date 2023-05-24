using GameShop.BLL.DTO.StrategyDTOs;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Strategies.PaymentStrategies
{
    public class IBoxStrategy : IPaymentStrategy
    {
        public PaymentResultDTO Pay(Order newOrder)
        {
            var result = new PaymentResultDTO
            {
                OrderId = newOrder.Id,
                IsPaymentSuccessful = true
            };

            return result;
        }
    }
}
