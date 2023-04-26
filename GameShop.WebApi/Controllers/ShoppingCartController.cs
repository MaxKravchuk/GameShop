using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services.Interfaces.Utils;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/shopingcart")]
    public class ShoppingCartController : ApiController
    {
        private readonly IDistributedCacheProvider _distributedCacheProvider;

        public ShoppingCartController(IDistributedCacheProvider distributedCacheProvider)
        {
            _distributedCacheProvider = distributedCacheProvider;
        }

        [HttpPost]
        [Route("addToCart")]
        public async Task<IHttpActionResult> AddGameToCart(CartItem cartItem)
        {
            await _distributedCacheProvider.AddCartItemAsync(cartItem);
            return Ok();
        }

        [HttpGet]
        [Route()]
        public async Task<IHttpActionResult> GetCache()
        {
            return Json(await _distributedCacheProvider.GetCartItemsAsync());
        }
    }
}
