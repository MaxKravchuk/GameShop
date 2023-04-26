using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.RedisDTOs;

namespace GameShop.BLL.Services.Interfaces.Utils
{
    public interface IDistributedCacheProvider
    {
        Task AddCartItemAsync(CartItem cartItem);

        Task<List<CartItem>> GetCartItemsAsync();
    }
}
