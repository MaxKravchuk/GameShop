using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(OrderCreateDTO orderCreateDTO);
    }
}
