using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GameShop.DAL.Context;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.DAL.Repository
{
    public class StoredProceduresProvider : IStoredProceduresProvider
    {
        private readonly GameShopContext _context;

        public StoredProceduresProvider(GameShopContext context)
        {
            _context = context;
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _context.Comments.SqlQuery("EXEC GetById @CommentId", new SqlParameter("@CommentId", id))
                .FirstOrDefaultAsync();
        }

        public async Task<Game> GetGameByKeyAsync(string gameKey)
        {
            return await _context.Games.SqlQuery("EXEC GetGameByKey @GameKey", new SqlParameter("@GameKey", gameKey))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Game>> GetGameByGenreIdAsync(int genreId)
        {
            return await _context.Games.SqlQuery("EXEC GetGamesByGenreId @GenreId", new SqlParameter("@GenreId", genreId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetGameByPlatformTypeIdAsync(int platformTypeId)
        {
            return await _context.Games.SqlQuery("EXEC GetGamesByPlatformTypeId @PlatformTypeId", new SqlParameter("@PlatformTypeId", platformTypeId))
                .ToListAsync();
        }

        public async Task<Genre> GetGenreByIdAsync(int genreId)
        {
            return await _context.Genres.SqlQuery("EXEC GetGenreById @GenreId", new SqlParameter("@GenreId", genreId))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            return await _context.Genres.SqlQuery("EXEC GetGenres")
                .ToListAsync();
        }
    }
}
