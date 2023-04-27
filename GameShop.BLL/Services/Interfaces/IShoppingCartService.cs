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
        Task AddCartItemAsync(CartItem cartItem);

        Task<IEnumerable<CartItem>> GetCartItemsAsync();

        Task DeletItemFromList(string gameKey);
    }
}
