using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/shopingcart")]
    public class ShoppingCartController : ApiController
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost]
        [Route("addToCart")]
        public async Task<IHttpActionResult> AddGameToCart(CartItem cartItem)
        {
            await _shoppingCartService.AddCartItemAsync(cartItem);
            return Ok();
        }

        [HttpGet]
        [Route()]
        public async Task<IHttpActionResult> GetCache()
        {
            return Json(await _shoppingCartService.GetCartItemsAsync());
        }
    }
}
