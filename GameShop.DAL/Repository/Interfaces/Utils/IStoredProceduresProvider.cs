using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.DAL.Entities;

namespace GameShop.DAL.Repository.Interfaces
{
    public interface IStoredProceduresProvider
    {
        Task<Comment> GetCommentByIdAsync(int id);

        Task<Game> GetGameByKeyAsync(string gameKey);

        Task<IEnumerable<Game>> GetGameByGenreIdAsync(int genreId);

        Task<IEnumerable<Game>> GetGameByPlatformTypeIdAsync(int platformTypeId);

        Task<Genre> GetGenreByIdAsync(int genreId);

        Task<IEnumerable<Genre>> GetGenresAsync();
    }
}
