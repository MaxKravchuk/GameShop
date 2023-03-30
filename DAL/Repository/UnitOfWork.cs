using DAL.Context;
using DAL.Entities;
using DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private GameShopContext _context;
        private Repository<Coment> _comentRepository;
        private Repository<Game> _gameRepository;
        private Repository<Genre> _genreRepository;
        private Repository<PlatformType> _platformTypeRepository;
        private bool disposed = false;

        public Repository<Coment> ComentRepository
        {
            get
            {
                if (_comentRepository == null)
                {
                    _comentRepository = new Repository<Coment>(_context);
                }
                return _comentRepository;
            }
        }
        public Repository<Game> GameRepository
        {
            get
            {
                if (_gameRepository == null)
                {
                    _gameRepository = new Repository<Game>(_context);
                }
                return _gameRepository;
            }
        }
        public Repository<Genre> GenreRepository
        {
            get
            {
                if (_genreRepository == null)
                {
                    _genreRepository = new Repository<Genre>(_context);
                }
                return _genreRepository;
            }
        }
        public Repository<PlatformType> PlatformTypeRepository
        {
            get
            {
                if (_platformTypeRepository == null)
                {
                    _platformTypeRepository = new Repository<PlatformType>(_context);
                }
                return _platformTypeRepository;
            }
        }
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
