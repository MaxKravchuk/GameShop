using AutoMapper;
using BAL.Services.Interfaces;
using BAL.ViewModels.GameViewModels;
using DAL.Entities;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GameShop.Controllers
{
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public GameController(
            IGameService gameService,
            IMapper mapper)
        {
            _gameService = gameService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("create")]
        public async Task CreateGameAsync([FromBody] GameCreateViewModel gameCreateViewModel)
        {
            var gameToCreate = _mapper.Map<Game>(gameCreateViewModel);
            await _gameService.Create(gameToCreate, gameCreateViewModel.GenresName, gameCreateViewModel.PlatformTypeName);
        }

        [HttpPut]
        [Route("update")]
        public async Task UpdateGameAsync([FromBody] GameUpdateViewModel gameUpdateViewModel)
        {
            var gameToUpdate = _mapper.Map<Game>(gameUpdateViewModel);
            await _gameService.Update(gameToUpdate);
        }

        [HttpGet]
        [Route("getDetailsByKey")]
        public async Task<GameReadViewModel> GetGameDetailsByKeyAsync([FromUri] string gameKey)
        {
            var game = await _gameService.GetByKeyAsync(gameKey);
            var model = _mapper.Map<GameReadViewModel>(game);
            return model;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IEnumerable<GameReadListViewModel>> GetAllGamesAsync(string search = null)
        {
            var games = await _gameService.GetAsync(search);
            var model = _mapper.Map<IEnumerable<GameReadListViewModel>>(games);
            return model;
        }

        [HttpDelete]
        [Route("delete")]
        public async Task DeleteGameAsync([FromUri] string gameKey)
        {
            await _gameService.Delete(gameKey);
        }

        [HttpGet]
        [Route("downloadGame")]
        public async Task<HttpResponseMessage> DownloadGame(string gameKey)
        {
            var game = await _gameService.GenerateGameFile(gameKey);
            string fileName = string.Format($"{gameKey}.bin");

            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new StreamContent(game);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = fileName;
            return response;
        }
    }
}