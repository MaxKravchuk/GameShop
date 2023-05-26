using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services.Interfaces;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/shoppingcart")]
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
        [Route("getAll")]
        public async Task<IHttpActionResult> GetGamesFromCartAsync()
        {
            return Json(await _shoppingCartService.GetCartItemsAsync());
        }

        [HttpDelete]
        [Route("delete/{gameKey}")]
        public async Task<IHttpActionResult> DeleteGameFromCartAsync(string gameKey)
        {
            await _shoppingCartService.DeleteItemFromListAsync(gameKey);
            return Ok();
        }

        [HttpGet]
        [Route("numberOfGames/{gameKey}")]
        public async Task<IHttpActionResult> GetNumberOfGamesInCartByGameKeyAsync(string gameKey)
        {
            var numberOfGames = await _shoppingCartService.GetNumberOfGamesByGameKeyAsync(gameKey);
            return Json(numberOfGames);
        }
    }
}
