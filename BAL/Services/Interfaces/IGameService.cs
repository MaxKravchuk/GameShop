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
        Task Create(Game game, IEnumerable<int> gameGenres, IEnumerable<int> gamePlatformTypes);
        Task<Game> GetByIdAsync(int id);
        Task<IEnumerable<Game>> GetAsync(string search = "");
        Task Update(Game game);
        Task Delete(int id);
        Task<Stream> GenerateGameFile(int id);
    }
}
