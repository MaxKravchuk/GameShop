using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Strategies.Interfaces;

namespace GameShop.BLL.Strategies
{
    public class PaymentContext<T> : IPaymentContext<T>
    {
        private IPaymentStrategy<T> _paymentStrategy;

        public PaymentContext(IPaymentStrategy<T> paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public async Task<T> ExecuteStrategy(OrderCreateDTO orderCreateDTO, IOrderService orderService)
        {
            return await _paymentStrategy.Pay(orderCreateDTO, orderService);
        }
    }
}
