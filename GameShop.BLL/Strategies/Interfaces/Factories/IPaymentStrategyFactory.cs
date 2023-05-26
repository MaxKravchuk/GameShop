using GameShop.BLL.Enums;
using GameShop.BLL.Strategies.Interfaces.Strategies;

namespace GameShop.BLL.Strategies.Interfaces.Factories
{
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategy GetPaymentStrategy(PaymentTypes paymentType);
    }
}
