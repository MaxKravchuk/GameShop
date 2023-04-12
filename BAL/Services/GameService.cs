using AutoMapper;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.DAL.Repository.Interfaces;
using GameShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(GameCreateDTO newGameDTO)
        {
            var gameToAdd = _mapper.Map<Game>(newGameDTO);

            var allGenres = await _unitOfWork.GenreRepository.GetAsync(
                filter: g=>newGameDTO.GenresId.Contains(g.Id));

            var allPlatformTypes = await _unitOfWork.PlatformTypeRepository.GetAsync(
                filter: plt => newGameDTO.PlatformTypeId.Contains(plt.Id));

            foreach (var genreId in newGameDTO.GenresId)
            {
                var genreToAdd = allGenres.SingleOrDefault(g => g.Id == genreId);
                
                if(genreToAdd == null)
                {
                    throw new NotFoundException();
                }

                gameToAdd.GameGenres.Add(genreToAdd);
            }
            foreach (var platformTypeId in newGameDTO.PlatformTypeId)
            {
                var platformTypeToAdd = allPlatformTypes.SingleOrDefault(plt => plt.Id == platformTypeId);
                
                if(platformTypeToAdd == null)
                {
                    throw new NotFoundException();
                }

                gameToAdd.GamePlatformTypes.Add(platformTypeToAdd);
            }

            _unitOfWork.GameRepository.Insert(gameToAdd);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(string gameKey)
        {
            var gamesToDelete = await _unitOfWork.GameRepository.GetAsync(
                filter: g => g.Key == gameKey);

            var gameToDelete = gamesToDelete.SingleOrDefault();

            if (gameToDelete == null)
            {
                throw new NotFoundException();
            }

            _unitOfWork.GameRepository.Delete(gameToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task<GameReadDTO> GetGameByKeyAsync(string gameKey)
        {
            var game = await _unitOfWork.GameRepository.GetAsync(
                filter:g=>g.Key==gameKey,includeProperties: "GamePlatformTypes,GameGenres");

            if(game.SingleOrDefault() == null)
            {
                throw new NotFoundException();
            }

            var model = _mapper.Map<GameReadDTO>(game.SingleOrDefault());
            return model;
        }

        public async Task<IEnumerable<GameReadListDTO>> GetAllGamesAsync()
        {
            var games = await _unitOfWork.GameRepository.GetAsync();
            
            var models = _mapper.Map<IEnumerable<GameReadListDTO>>(games);
            return models;
        }

        public async Task<IEnumerable<GameReadListDTO>> GetGamesByGenreAsync(int genreId)
        {
            var games = await _unitOfWork.GameRepository.GetAsync(
                filter: g => g.GameGenres.Any(gg => gg.Id == genreId));

            var models = _mapper.Map<IEnumerable<GameReadListDTO>>(games);
            return models;
        }

        public async Task<IEnumerable<GameReadListDTO>> GetGamesByPlatformTypeAsync(int platformTypeId)
        {
            var games = await _unitOfWork.GameRepository.GetAsync(filter:
                    g => g.GamePlatformTypes.Any(gg => gg.Id == platformTypeId));

            var models = _mapper.Map<IEnumerable<GameReadListDTO>>(games);
            return models;
        }

        public async Task UpdateAsync(GameUpdateDTO updatedGameDTO)
        {
            var exGame = (await _unitOfWork.GameRepository.GetAsync(
                filter:game=>game.Key==updatedGameDTO.Key,
                includeProperties: "GameGenres,GamePlatformTypes")).SingleOrDefault();

            _mapper.Map(updatedGameDTO, exGame);

            if (exGame == null)
            {
                throw new BadRequestException();
            }

            exGame.GamePlatformTypes.Clear();
            exGame.GameGenres.Clear();

            var allGenres = await _unitOfWork.GenreRepository.GetAsync(
                filter: g => updatedGameDTO.GenresId.Contains(g.Id));

            var allPlatformTypes = await _unitOfWork.PlatformTypeRepository.GetAsync(
                filter: plt => updatedGameDTO.PlatformTypeId.Contains(plt.Id));

            foreach (var genreId in updatedGameDTO.GenresId)
            {
                var genreToAdd = allGenres.SingleOrDefault(g => g.Id == genreId);

                if (genreToAdd == null)
                {
                    throw new NotFoundException();
                }

                exGame.GameGenres.Add(genreToAdd);
            }

            foreach (var platformTypeId in updatedGameDTO.PlatformTypeId)
            {
                var platformTypeToAdd = allPlatformTypes.SingleOrDefault(plt => plt.Id == platformTypeId);

                if (platformTypeToAdd == null)
                {
                    throw new NotFoundException();
                }

                exGame.GamePlatformTypes.Add(platformTypeToAdd);
            }


            _unitOfWork.GameRepository.Update(exGame);
            await _unitOfWork.SaveAsync();
        }

        public async Task<MemoryStream> GenerateGameFileAsync(string key)
        {
            var stringInMemoryStream = new MemoryStream(Encoding.ASCII.GetBytes(key));
            await Task.Yield();
            return stringInMemoryStream;
        }
    }
}
