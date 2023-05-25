using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.PaymentDTOs;
using GameShop.BLL.DTO.StrategyDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResultDTO> ExecutePaymentAsync(PaymentCreateDTO paymentCreateDTO);
    }
}
