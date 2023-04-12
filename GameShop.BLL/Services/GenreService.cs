using AutoMapper;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenreService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(GenreCreateDTO genreToAddDTO)
        {
            var genreToAdd = _mapper.Map<Genre>(genreToAddDTO);
            _unitOfWork.GenreRepository.Insert(genreToAdd);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var genreToDelete = await _unitOfWork.GenreRepository.GetByIdAsync(id);
            
            if(genreToDelete == null)
            {
                throw new NotFoundException();
            }

            _unitOfWork.GenreRepository.Delete(genreToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<GenreReadListDTO>> GetAsync(string gameKey = "")
        {
            var genres = await _unitOfWork.GenreRepository.GetAsync(
                filter: g=>g.GameGenres.Any(gg=>gg.Key==gameKey));
            
            var genresDTO = _mapper.Map<IEnumerable<GenreReadListDTO>>(genres);
            return genresDTO;
        }

        public async Task<GenreReadDTO> GetByIdAsync(int id)
        {
            var genre = await _unitOfWork.GenreRepository.GetByIdAsync(id);

            if(genre == null)
            {
                throw new NotFoundException();
            }

            var genreDTO = _mapper.Map<GenreReadDTO>(genre);
            return genreDTO;
        }

        public async Task UpdateAsync(GenreUpdateDTO genreToUpdateDTO)
        {
            var genreToUpdate = await _unitOfWork.GenreRepository.GetByIdAsync(genreToUpdateDTO.Id);

            if (genreToUpdate == null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(genreToUpdateDTO, genreToUpdate);
            
            _unitOfWork.GenreRepository.Update(genreToUpdate);
            await _unitOfWork.SaveAsync();
        }
    }
}
