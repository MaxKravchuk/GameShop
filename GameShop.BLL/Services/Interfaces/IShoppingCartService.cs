using System.Collections.Generic;
using System.Threading.Tasks;
using GameShop.BLL.DTO.RedisDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task AddCartItemAsync(CartItemDTO cartItem);

        Task<IEnumerable<CartItemDTO>> GetCartItemsAsync(int customerId);

        Task<int> GetNumberOfGamesByGameKeyAsync(int customerId, string gameKey);

        Task DeleteItemFromListAsync(int customerId, string gameKey);

        Task CleatCartAsync(int customerId);
    }
}
