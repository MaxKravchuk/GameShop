using BAL.Exceptions;
using BAL.Services.Interfaces;
using BAL.ViewModels.GenreViewModels;
using DAL.Entities;
using DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IRepository<Genre> _genreRepository;

        public GenreService(IUnitOfWork unitOfWork)
        {
            _genreRepository = unitOfWork.GenreRepository;
        }

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Task<GenreReadListViewModel> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GenreReadViewModel> GetAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Genre> GetByNameAsync(object key)
        {
            var genre = await _genreRepository.GetAsync(key,
                includeProperties: "ParentGenre,SubGenres");

            if(genre == null)
            {
                throw new NotFoundException();
            }

            return genre;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
