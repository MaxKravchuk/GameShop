using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Filters;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/shoppingcart")]
    [JwtAuthorize]
    public class ShoppingCartController : ApiController
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost]
        [Route("addToCart")]
        public async Task<IHttpActionResult> AddGameToCartAsync(CartItemDTO cartItem)
        {
            await _shoppingCartService.AddCartItemAsync(cartItem);
            return Ok();
        }

        [HttpGet]
        [Route("getAll/{customerId}")]
        public async Task<IHttpActionResult> GetGamesFromCartAsync(int customerId)
        {
            return Json(await _shoppingCartService.GetCartItemsAsync(customerId));
        }

        [HttpDelete]
        [Route("delete/{customerId}-{gameKey}")]
        public async Task<IHttpActionResult> DeleteGameFromCartAsync(int customerId, string gameKey)
        {
            await _shoppingCartService.DeleteItemFromListAsync(customerId, gameKey);
            return Ok();
        }

        [HttpGet]
        [Route("numberOfGames/{customerId}-{gameKey}")]
        public async Task<IHttpActionResult> GetNumberOfGamesInCartByGameKeyAsync(int customerId, string gameKey)
        {
            var numberOfGames = await _shoppingCartService.GetNumberOfGamesByGameKeyAsync(customerId, gameKey);
            return Json(numberOfGames);
        }
    }
}
