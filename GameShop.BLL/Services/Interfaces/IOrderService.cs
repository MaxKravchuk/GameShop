using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.StrategyDTOs;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<PaymentResultDTO> ExecutePayment(OrderCreateDTO orderCreateDTO);
    }
}
