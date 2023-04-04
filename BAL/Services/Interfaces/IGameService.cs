using BAL.ViewModels.GameViewModels;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IGameService
    {
        Task Create(Game game, IEnumerable<string> gameGenres, IEnumerable<string> gamePlatformTypes);
        Task<Game> GetByKeyAsync(object key);
        Task<IEnumerable<Game>> GetAsync(string search);
        Task Update(Game game);
        Task Delete(object id);
        Task<Stream> GenerateGameFile(object gameKey);
    }
}
