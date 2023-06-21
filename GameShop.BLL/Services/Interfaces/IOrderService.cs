using System.Collections.Generic;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(OrderCreateDTO orderCreateDTO);

        Task<IEnumerable<OrderReadListDTO>> GetAllOrdersAsync();

        Task<OrderReadDTO> GetOrderByIdAsync(int orderId);

        Task UpdateOrderStatusAsync(OrderUpdateDTO orderUpdateDTO);

        Task UpdateOrderDetailsAsync(OrderUpdateDTO orderUpdateDTO);
    }
}
