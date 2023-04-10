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
        private Lazy<IRepository<Comment>> _comentRepository;
        private Lazy<IRepository<Game>> _gameRepository;
        private Lazy<IRepository<Genre>> _genreRepository;
        private Lazy<IRepository<PlatformType>> _platformTypeRepository;
        private bool disposed = false;

        public UnitOfWork(GameShopContext context)
        {
            _context = context;
            _comentRepository = new Lazy<IRepository<Comment>>(() => new Repository<Comment>(_context));
            _gameRepository = new Lazy<IRepository<Game>>(() => new Repository<Game>(_context));
            _genreRepository = new Lazy<IRepository<Genre>>(() => new Repository<Genre>(_context));
            _platformTypeRepository = new Lazy<IRepository<PlatformType>>(() => new Repository<PlatformType>(_context));
        }

        public IRepository<Comment> ComentRepository => _comentRepository.Value;

        public IRepository<Game> GameRepository => _gameRepository.Value;

        public IRepository<Genre> GenreRepository => _genreRepository.Value;

        public IRepository<PlatformType> PlatformTypeRepository => _platformTypeRepository.Value;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
