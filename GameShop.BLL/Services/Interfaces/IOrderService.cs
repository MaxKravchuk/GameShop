using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.StrategyDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<PaymentResultDTO> ExecutePayment(OrderCreateDTO orderCreateDTO);
    }
}
