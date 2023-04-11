using GameShop.BLL.DTO.GameDTOs;
using GameShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IGameService
    {
        Task CreateAsync(GameCreateDTO newGameDTO);
        Task<GameReadDTO> GetGameByKeyAsync(string gameKey);
        Task<IEnumerable<GameReadListDTO>> GetAllGamesAsync();
        Task<IEnumerable<GameReadListDTO>> GetGamesByGenreAsync(int genreId);
        Task<IEnumerable<GameReadListDTO>> GetGamesByPlatformTypeAsync(int platformTypeId);
        Task UpdateAsync(GameUpdateDTO updatedGameDTO);
        Task DeleteAsync(string gameKey);
        Task<MemoryStream> GenerateGameFileAsync(string key);
    }
}
