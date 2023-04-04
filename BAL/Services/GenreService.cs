using BAL.Exceptions;
using BAL.Services.Interfaces;
using BAL.ViewModels.GenreViewModels;
using DAL.Entities;
using DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task Create(Genre genre)
        {
            _genreRepository.Insert(genre);
            await _genreRepository.SaveChangesAsync();
        }

        public async Task Delete(Genre genre)
        {
            _genreRepository.Delete(genre);
            await _genreRepository.SaveChangesAsync();

        }

        public async Task<IEnumerable<Genre>> GetAsync(string gameKey = "")
        {

            var filter = GetFilterQuery(gameKey);
            var genres = await _genreRepository.GetAsync(filter:filter);

            if(genres == null)
            {
                throw new NotFoundException();
            }

            return genres;
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id,
                includeProperties: "ParentGenre,SubGenres");

            if(genre == null)
            {
                throw new NotFoundException();
            }

            return genre;
        }

        public async Task Update(Genre genre)
        {
            _genreRepository.Update(genre);
            await _genreRepository.SaveChangesAsync();
        }

        private static Expression<Func<Genre, bool>> GetFilterQuery(string filterParam)
        {
            Expression<Func<Genre, bool>> filterQuery = null;

            if (filterParam is null) return filterQuery;

            var formattedFilter = filterParam.Trim().ToLower();

            filterQuery = u => u.GameGenres.All(gg=>gg.Key.ToLower().Contains(formattedFilter));

            return filterQuery;
        }
    }
}
