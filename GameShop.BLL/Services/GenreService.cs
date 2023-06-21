using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Pagination.Extensions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.BLL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IValidator<GenreCreateDTO> _validator;

        public GenreService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager loggerManager,
            IValidator<GenreCreateDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _validator = validator;
        }

        public async Task CreateAsync(GenreCreateDTO genreToAddDTO)
        {
            await _validator.ValidateAndThrowAsync(genreToAddDTO);

            var genreToAdd = _mapper.Map<Genre>(genreToAddDTO);
            if (genreToAddDTO.ParentGenreId != null)
            {
                genreToAdd.ParentGenreId = genreToAddDTO.ParentGenreId;
            }

            _unitOfWork.GenreRepository.Insert(genreToAdd);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Genre with name {genreToAdd.Name} was created successfully");
        }

        public async Task DeleteAsync(int id)
        {
            var genreToDelete = await _unitOfWork.GenreRepository.GetByIdAsync(id);

            if (genreToDelete == null)
            {
                throw new NotFoundException($"Genre with id {id} was not found");
            }

            var games = await _unitOfWork.GameRepository
                .GetAsync(
                filter: g => g.GameGenres.Any(gg => gg.Name == genreToDelete.Name),
                includeProperties: "GameGenres");
            foreach (var game in games)
            {
                game.GameGenres.Remove(genreToDelete);
                _unitOfWork.GameRepository.Update(game);
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

        public async Task<PagedListDTO<GenreReadListDTO>> GetPagedAsync(PaginationRequestDTO paginationRequestDTO)
        {
            var genres = await _unitOfWork.GenreRepository.GetAsync();

            var pagedGenres = genres.ToPagedList(paginationRequestDTO.PageNumber, paginationRequestDTO.PageSize);
            var pagedModels = _mapper.Map<PagedListDTO<GenreReadListDTO>>(pagedGenres);

            _loggerManager.LogInfo(
                $"Genres were returned successfully in array size of {pagedModels.Entities.Count()}");
            return pagedModels;
        }

        public async Task UpdateAsync(GenreUpdateDTO genreToUpdateDTO)
        {
            await _validator.ValidateAndThrowAsync(genreToUpdateDTO);

            var genreToUpdate = await _unitOfWork.GenreRepository.GetByIdAsync(genreToUpdateDTO.Id);

            if (genreToUpdate == null)
            {
                throw new NotFoundException($"Genre with id {genreToUpdateDTO.Id} was not found");
            }

            if (genreToUpdateDTO.ParentGenreId != null)
            {
                genreToUpdate.ParentGenreId = genreToUpdateDTO.ParentGenreId;
            }

            _mapper.Map(genreToUpdateDTO, genreToUpdate);

            _unitOfWork.GenreRepository.Update(genreToUpdate);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Genre with id {genreToUpdateDTO.Id} was updated successfully");
        }
    }
}
