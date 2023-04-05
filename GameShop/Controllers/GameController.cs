using AutoMapper;
using BAL.Services.Interfaces;
using BAL.ViewModels.GameViewModels;
using DAL.Entities;
using DAL.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.WebPages;

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
            await _gameService.Create(gameToCreate, gameCreateViewModel.GenresId, gameCreateViewModel.PlatformTypeId);
        }

        [HttpPut]
        [Route("update")]
        public async Task UpdateGameAsync([FromBody] GameUpdateViewModel gameUpdateViewModel)
        {
            var gameToUpdate = _mapper.Map<Game>(gameUpdateViewModel);
            await _gameService.Update(gameToUpdate, gameUpdateViewModel.GenresId,gameUpdateViewModel.PlatformTypeId);
        }

        [HttpGet]
        [Route("getDetailsById")]
        public async Task<GameReadViewModel> GetGameDetailsByKeyAsync([FromUri] string gameKey)
        {
            var game = await _gameService.GetByKeyGameAsync(gameKey);
            var model = _mapper.Map<GameReadViewModel>(game);
            return model;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IEnumerable<GameReadListViewModel>> GetAllGamesAsync()
        {
            var games = await _gameService.GetAllGamesAsync();
            var model = _mapper.Map<IEnumerable<GameReadListViewModel>>(games);
            return model;
        }

        [HttpGet]
        [Route("getByGenreOrPlatformType")]
        public async Task<IEnumerable<GameReadListViewModel>> GetAllGamesByParameterAsync([FromUri]GameParameters gameParameters)
        {
            var games = await _gameService.GetGameByGenreOrPltAsync(gameParameters);
            var model = _mapper.Map<IEnumerable<GameReadListViewModel>>(games);
            return model;
        }

        [HttpDelete]
        [Route("delete")]
        public async Task DeleteGameAsync([FromUri] int gameId)
        {
            await _gameService.Delete(gameId);
        }

        [HttpGet]
        [Route("downloadGame")]
        public HttpResponseMessage DownloadGame()
        {
            string Rpath = @"C:\DiscD\";
            string path = Path.Combine(Rpath, "game.bin");

            return _gameService.GenerateGameFile(path);
        }
    }
}