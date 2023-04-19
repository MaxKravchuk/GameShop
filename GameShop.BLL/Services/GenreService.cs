using AutoMapper;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public GenreService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager loggerManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        public async Task CreateAsync(GenreCreateDTO genreToAddDTO)
        {
            var genreToAdd = _mapper.Map<Genre>(genreToAddDTO);
            _unitOfWork.GenreRepository.Insert(genreToAdd);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Genre with name {genreToAdd.Name} was created successfully");
        }

        public async Task DeleteAsync(int id)
        {
            var genreToDelete = await _unitOfWork.GenreRepository.GetByIdAsync(id);

            if (genreToDelete == null)
            {
                throw new NotFoundException($"Genre with id {id} does not found");
            }

            _unitOfWork.GenreRepository.Delete(genreToDelete);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Genre with id {id} was deleted successfully");
        }

        public async Task<IEnumerable<GenreReadListDTO>> GetAsync()
        {
            var genres = await _unitOfWork.GenreRepository.GetAsync();

            var genresDTO = _mapper.Map<IEnumerable<GenreReadListDTO>>(genres);

            _loggerManager.LogInfo(
                $"Genres were returned successfully in array size of {genresDTO.Count()}");
            return genresDTO;
        }

        public async Task<GenreReadDTO> GetByIdAsync(int id)
        {
            var genre = await _unitOfWork.GenreRepository.GetByIdAsync(id);

            if (genre == null)
            {
                throw new NotFoundException($"Genre with id {id} does not found");
            }

            var genreDTO = _mapper.Map<GenreReadDTO>(genre);

            _loggerManager.LogInfo($"Genre with id {id} successfully returned");
            return genreDTO;
        }

        public async Task UpdateAsync(GenreUpdateDTO genreToUpdateDTO)
        {
            var genreToUpdate = await _unitOfWork.GenreRepository.GetByIdAsync(genreToUpdateDTO.Id);

            if (genreToUpdate == null)
            {
                throw new NotFoundException($"Genre with id {genreToUpdateDTO.Id} does not found");
            }

            _mapper.Map(genreToUpdateDTO, genreToUpdate);

            _unitOfWork.GenreRepository.Update(genreToUpdate);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Genre with id {genreToUpdateDTO.Id} was updated successfully");
        }
    }
}
