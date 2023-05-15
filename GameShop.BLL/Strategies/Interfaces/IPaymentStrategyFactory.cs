using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Strategies.Interfaces
{
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategy GetPaymentStrategy(string paymentType);
    }
}
