using System;
using System.Threading.Tasks;
using GameShop.DAL.Context;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly GameShopContext _context;
        private readonly Lazy<IRepository<Comment>> _commentRepository;
        private readonly Lazy<IRepository<Game>> _gameRepository;
        private readonly Lazy<IRepository<Genre>> _genreRepository;
        private readonly Lazy<IRepository<PlatformType>> _platformTypeRepository;
        private readonly Lazy<IRepository<Publisher>> _publisherReposiroty;
        private readonly Lazy<IRepository<Order>> _orderReposiroty;
        private readonly Lazy<IRepository<OrderDetails>> _orderDetailsReposiroty;
        private bool _disposed = false;

        public UnitOfWork(
            GameShopContext context,
            Lazy<IRepository<Comment>> commentRepository,
            Lazy<IRepository<Game>> gameRepository,
            Lazy<IRepository<Genre>> genreRepository,
            Lazy<IRepository<PlatformType>> platformTypeRepository,
            Lazy<IRepository<Publisher>> publisherReposiroty,
            Lazy<IRepository<Order>> orderReposiroty,
            Lazy<IRepository<OrderDetails>> orderDetailsReposiroty)
        {
            _context = context;
            _commentRepository = commentRepository;
            _gameRepository = gameRepository;
            _genreRepository = genreRepository;
            _platformTypeRepository = platformTypeRepository;
            _publisherReposiroty = publisherReposiroty;
            _orderReposiroty = orderReposiroty;
            _orderDetailsReposiroty = orderDetailsReposiroty;
        }

        public IRepository<Comment> CommentRepository => _commentRepository.Value;

        public IRepository<Game> GameRepository => _gameRepository.Value;

        public IRepository<Genre> GenreRepository => _genreRepository.Value;

        public IRepository<PlatformType> PlatformTypeRepository => _platformTypeRepository.Value;

        public IRepository<Publisher> PublisherRepository => _publisherReposiroty.Value;

        public IRepository<Order> OrderRepository => _orderReposiroty.Value;

        public IRepository<OrderDetails> OrderDetailsRepository => _orderDetailsReposiroty.Value;

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
