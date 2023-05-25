using GameShop.BLL.Enums;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Strategies.Interfaces.Factories;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.BLL.Strategies.PaymentStrategies;
using Unity;

namespace GameShop.BLL.Strategies.Factories
{
    public class PaymentStrategyFactory : IPaymentStrategyFactory
    {
        private readonly IUnityContainer _container;

        public PaymentStrategyFactory(IUnityContainer container)
        {
            _container = container;
        }

        public IPaymentStrategy GetPaymentStrategy(PaymentTypes paymentType)
        {
            switch (paymentType)
            {
                case PaymentTypes.Visa:
                    return _container.Resolve<VisaStrategy>();
                case PaymentTypes.Bank:
                    return _container.Resolve<BankStrategy>();
                case PaymentTypes.IBox:
                    return _container.Resolve<IBoxStrategy>();
                default:
                    throw new BadRequestException("Invalid payment type");
            }
        }
    }
}
