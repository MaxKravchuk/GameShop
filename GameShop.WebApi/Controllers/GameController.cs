using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.DTO.FilterDTOs;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Filters;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/games")]
    public class GameController : ApiController
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        [Route()]
        public async Task<IHttpActionResult> CreateGameAsync([FromBody] GameCreateDTO gameCreateViewModel)
        {
            await _gameService.CreateAsync(gameCreateViewModel);
            return Ok();
        }

        [HttpPut]
        [Route("update")]
        public async Task<IHttpActionResult> UpdateGameAsync([FromBody] GameUpdateDTO gameUpdateViewModel)
        {
            await _gameService.UpdateAsync(gameUpdateViewModel);
            return Ok();
        }

        [HttpGet]
        [Route("getDetailsByKey/{gameKey}")]
        [JwtAuthorize]
        public async Task<IHttpActionResult> GetGameDetailsByKeyAsync(string gameKey)
        {
            var game = await _gameService.GetGameByKeyAsync(gameKey);
            return Json(game);
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAllGamesAsync([FromUri] GameFiltersDTO gameFiltersDTO)
        {
            var games = await _gameService.GetAllGamesAsync(gameFiltersDTO);
            return Json(games);
        }

        [HttpGet]
        [Route("getByGenre/{genreId}")]
        public async Task<IHttpActionResult> GetAllGamesByGenreAsync(int genreId)
        {
            var games = await _gameService.GetGamesByGenreAsync(genreId);
            return Json(games);
        }

        [HttpGet]
        [Route("getByPlatformType/{platformTypeId}")]
        public async Task<IHttpActionResult> GetAllGamesByPlatformTypeAsync(int platformTypeId)
        {
            var games = await _gameService.GetGamesByPlatformTypeAsync(platformTypeId);
            return Json(games);
        }

        [HttpDelete]
        [Route("delete/{gameKey}")]
        public async Task<IHttpActionResult> DeleteGameAsync(string gameKey)
        {
            await _gameService.DeleteAsync(gameKey);
            return Ok();
        }

        [HttpGet]
        [Route("downloadGame/{gameKey}")]
        public async Task<HttpResponseMessage> DownloadGameAsync(string gameKey)
        {
            var stream = await _gameService.GenerateGameFileAsync(gameKey);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentLength = stream.Length;
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = $"{gameKey}.bin";

            return result;
        }

        [HttpGet]
        [Route("numberOfGames")]
        public async Task<IHttpActionResult> GetNumberOfGamesAsync()
        {
            var number = await _gameService.GetNumberOfGamesAsync();
            return Ok(number);
        }
    }
}
