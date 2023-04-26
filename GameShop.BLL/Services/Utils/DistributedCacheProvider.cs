using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services.Interfaces.Utils;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GameShop.BLL.Services.Utils
{
    public class DistributedCacheProvider : IDistributedCacheProvider
    {
        private readonly IDatabase _database;
        private readonly string _redisConnectionString;

        public DistributedCacheProvider()
        {
            _redisConnectionString = ConfigurationManager.ConnectionStrings["RedisConnectingString"].ConnectionString;
            var redisConnection = ConnectionMultiplexer.Connect(_redisConnectionString);
            _database = redisConnection.GetDatabase();
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            var existingCartItem = await _database.HashGetAsync("CartItems", cartItem.GameKey);
            if (existingCartItem.HasValue)
            {
                var existingCartItemObject = JsonConvert.DeserializeObject<CartItem>(existingCartItem);
                existingCartItemObject.Quantity += 1;
                var updatedCartItemJson = JsonConvert.SerializeObject(existingCartItemObject);
                await _database.HashSetAsync("CartItems", cartItem.GameKey, updatedCartItemJson);
            }
            else
            {
                cartItem.Quantity = 1;
                var newCartItemJson = JsonConvert.SerializeObject(cartItem);
                await _database.HashSetAsync("CartItems", cartItem.GameKey, newCartItemJson);
            }
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            var cartItemsJson = await _database.HashValuesAsync("CartItems");
            var cartItems = cartItemsJson.Select(x => JsonConvert.DeserializeObject<CartItem>(x.ToString())).ToList();
            return cartItems;
        }
    }
}
