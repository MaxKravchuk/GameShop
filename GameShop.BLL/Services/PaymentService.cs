using System.Threading.Tasks;
using GameShop.BLL.DTO.PaymentDTOs;
using GameShop.BLL.DTO.StrategyDTOs;
using GameShop.BLL.Enums;
using GameShop.BLL.Enums.Extensions;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.BLL.Strategies.Interfaces.Factories;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _loggerManager;
        private readonly IPaymentStrategyFactory _paymentStrategyFactory;

        public PaymentService(
            IUnitOfWork unitOfWork,
            ILoggerManager loggerManager,
            IPaymentStrategyFactory paymentStrategyFactory)
        {
            _unitOfWork = unitOfWork;
            _loggerManager = loggerManager;
            _paymentStrategyFactory = paymentStrategyFactory;
        }

        public async Task<PaymentResultDTO> ExecutePaymentAsync(PaymentCreateDTO paymentCreateDTO)
        {
            var orderToPay = await _unitOfWork.OrderRepository.GetByIdAsync(id: paymentCreateDTO.OrderId);

            var strategyType = paymentCreateDTO.Strategy.ToEnum<PaymentTypes>();
            var strategy = _paymentStrategyFactory.GetPaymentStrategy(strategyType);

            var paymentResult = strategy.Pay(orderToPay);
            if (!paymentResult.IsPaymentSuccessful)
            {
                orderToPay.IsPaid = false;
                orderToPay.Status = OrderStatusTypes.Unpaid.ToString();
                throw new BadRequestException("Payment is not successful");
            }
            else
            {
                _loggerManager.LogInfo("Payment done successfully");
                orderToPay.IsPaid = true;
                orderToPay.Status = OrderStatusTypes.Paid.ToString();
            }

            _unitOfWork.OrderRepository.Update(orderToPay);
            await _unitOfWork.SaveAsync();

            return paymentResult;
        }
    }
}
