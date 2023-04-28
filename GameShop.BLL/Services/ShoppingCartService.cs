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
        private readonly IRedisProvider<CartItemDTO> _redisProvider;
        private readonly ILoggerManager _loggerManager;
        private readonly IValidator<CartItemDTO> _validator;

        public ShoppingCartService(
            IRedisProvider<CartItemDTO> redisProvider,
            ILoggerManager loggerManager,
            IValidator<CartItemDTO> validator)
        {
            _redisProvider = redisProvider;
            _loggerManager = loggerManager;
            _validator = validator;
        }

        public async Task AddCartItemAsync(CartItemDTO cartItem)
        {
            await _validator.ValidateAndThrowAsync(cartItem);

            var existingCartItem = await _redisProvider.GetValueAsync(_redisKey, cartItem.GameKey);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += 1;
                await _redisProvider.SetValueToListAsync("CartItems", cartItem.GameKey, existingCartItem);
                _loggerManager.LogInfo($"Item with key {cartItem.GameKey} updated");
            }
            else
            {
                await _redisProvider.SetValueToListAsync("CartItems", cartItem.GameKey, cartItem);
                _loggerManager.LogInfo($"New item with key {cartItem.GameKey} added to cart");
            }
        }

        public async Task<IEnumerable<CartItemDTO>> GetCartItemsAsync()
        {
            var item = await _redisProvider.GetValuesAsync(_redisKey);
            _loggerManager.LogInfo($"List of items returned with array length of {item.Count()}");
            return item;
        }

        public async Task DeletItemFromListAsync(string gameKey)
        {
            var existingCartItem = await _redisProvider.GetValueAsync(_redisKey, gameKey);
            if (existingCartItem.Quantity == 1)
            {
                await _redisProvider.DeleteItemFromListAsync(_redisKey, gameKey);
            }
            else
            {
                existingCartItem.Quantity -= 1;
                await _redisProvider.SetValueToListAsync("CartItems", gameKey, existingCartItem);
            }

            _loggerManager.LogInfo($"Item with game key {gameKey} is deleted");
        }
    }
}
