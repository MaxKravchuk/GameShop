using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.RedisDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task AddCartItemAsync(CartItemDTO cartItem);

        Task<IEnumerable<CartItemDTO>> GetCartItemsAsync();

        Task DeletItemFromListAsync(string gameKey);
    }
}
