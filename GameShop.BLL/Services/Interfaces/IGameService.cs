﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GameShop.BLL.DTO.FilterDTOs;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.DTO.PaginationDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IGameService
    {
        Task CreateAsync(GameCreateDTO newGameDTO);

        Task<GameReadDTO> GetGameByKeyAsync(string gameKey);

        Task<PagedListDTO<GameReadListDTO>> GetAllGamesAsync(GameFiltersDTO gameFiltersDTO);

        Task<IEnumerable<GameReadListDTO>> GetGamesByGenreAsync(int genreId);

        Task<IEnumerable<GameReadListDTO>> GetGamesByPlatformTypeAsync(int platformTypeId);

        Task<IEnumerable<GameReadListDTO>> GetGamesByPublisherAsync(int publisherId);

        Task UpdateAsync(GameUpdateDTO updatedGameDTO);

        Task DeleteAsync(string gameKey);

        Task<MemoryStream> GenerateGameFileAsync(string key);

        Task<int> GetNumberOfGamesAsync();
    }
}
