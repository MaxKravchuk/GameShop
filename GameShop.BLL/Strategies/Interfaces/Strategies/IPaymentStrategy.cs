using GameShop.BLL.DTO.StrategyDTOs;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Strategies.Interfaces.Strategies
{
    public interface IPaymentStrategy
    {
        PaymentResultDTO Pay(Order order);
    }
}
