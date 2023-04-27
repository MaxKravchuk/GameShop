using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Repository.Interfaces.Utils;

namespace GameShop.BLL.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private const string _redisKey = "CartItems";
        private readonly IRedisProvider<CartItem> _redisProvider;
        private readonly ILoggerManager _loggerManager;
        private readonly IValidator<CartItem> _validator;

        public ShoppingCartService(
            IRedisProvider<CartItem> redisProvider,
            ILoggerManager loggerManager,
            IValidator<CartItem> validator)
        {
            _redisProvider = redisProvider;
            _loggerManager = loggerManager;
            _validator = validator;
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _validator.ValidateAndThrowAsync(cartItem);

            var existingCartItem = await _redisProvider.GetValueAsync(_redisKey, cartItem.GameKey);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                await _redisProvider.SetValueToListASync("CartItems", cartItem.GameKey, existingCartItem);
                _loggerManager.LogInfo($"Item with key {cartItem.GameKey} updated");
            }
            else
            {
                await _redisProvider.SetValueToListASync("CartItems", cartItem.GameKey, cartItem);
                _loggerManager.LogInfo($"New item with key {cartItem.GameKey} added to cart");
            }
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync()
        {
            var item = await _redisProvider.GetValuesAsync(_redisKey);
            _loggerManager.LogInfo($"List of items returned with array length of {item.Count()}");
            return item;
        }
    }
}
