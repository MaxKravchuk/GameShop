using GameShop.DAL.Context;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private GameShopContext _context;
        private Lazy<IRepository<Comment>> _commentRepository;
        private Lazy<IRepository<Game>> _gameRepository;
        private Lazy<IRepository<Genre>> _genreRepository;
        private Lazy<IRepository<PlatformType>> _platformTypeRepository;
        private bool _disposed = false;

        public UnitOfWork(
            GameShopContext context,
            Lazy<IRepository<Comment>> commentRepository,
            Lazy<IRepository<Game>> gameRepository,
            Lazy<IRepository<Genre>> genreRepository,
            Lazy<IRepository<PlatformType>> platformTypeRepository)
        {
            _context = context;
            _commentRepository = commentRepository;
            _gameRepository = gameRepository;
            _genreRepository = genreRepository;
            _platformTypeRepository = platformTypeRepository;
        }

        public IRepository<Comment> CommentRepository => _commentRepository.Value;

        public IRepository<Game> GameRepository => _gameRepository.Value;

        public IRepository<Genre> GenreRepository => _genreRepository.Value;

        public IRepository<PlatformType> PlatformTypeRepository => _platformTypeRepository.Value;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }
    }
}
