using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Repository.Interfaces.Utils;
using Moq;
using Xunit;

namespace BLL.Test.ServiceTests
{
    public class ShoppingCartServiceTests
    {
        private readonly Mock<IRedisProvider<CartItem>> _mockRedisProvider;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly Mock<IValidator<CartItem>> _mockValidator;
        private readonly ShoppingCartService _shoppingCartService;
        private const string _redisKey = "CartItems";

        public ShoppingCartServiceTests()
        {
            _mockRedisProvider = new Mock<IRedisProvider<CartItem>>();
            _mockLogger = new Mock<ILoggerManager>();
            _mockValidator = new Mock<IValidator<CartItem>>();

            _shoppingCartService = new ShoppingCartService(
                _mockRedisProvider.Object,
                _mockLogger.Object,
                _mockValidator.Object);
        }
    }
}
