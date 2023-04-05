using BAL.ViewModels.GameViewModels;
using DAL.Entities;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IGameService
    {
        Task Create(Game game, IEnumerable<int> gameGenres, IEnumerable<int> gamePlatformTypes);
        Task<Game> GetByKeyGameAsync(string gameKey);
        Task<IEnumerable<Game>> GetAllGamesAsync();
        Task<IEnumerable<Game>> GetGameByGenreOrPltAsync(GameParameters gameParameters);
        Task Update(Game game, IEnumerable<int> genresId, IEnumerable<int> platformTypesId);
        Task Delete(int id);
        HttpResponseMessage GenerateGameFile(string path);
    }
}
