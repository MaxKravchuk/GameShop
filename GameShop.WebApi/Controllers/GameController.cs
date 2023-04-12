using AutoMapper;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.DAL.Entities;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.WebPages;
using System.Net;
using System.Runtime.InteropServices.ComTypes;

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
        public async Task<IHttpActionResult> GetGameDetailsByKeyAsync(string gameKey)
        {
            var game = await _gameService.GetGameByKeyAsync(gameKey);
            return Json(game);
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAllGamesAsync()
        {
            var games = await _gameService.GetAllGamesAsync();
            return Json(games);
        }

        [HttpGet]
        [Route("getByGenre/{genreId}")]
        public async Task<IHttpActionResult> GetAllGamesByGenre(int genreId)
        {
            var games = await _gameService.GetGamesByGenreAsync(genreId);
            return Json(games);
        }

        [HttpGet]
        [Route("getByPlatformType/{platformTypeId}")]
        public async Task<IHttpActionResult> GetAllGamesByPlatformType(int platformTypeId)
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
        public async Task<HttpResponseMessage> DownloadGame(string gameKey)
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
    }
}