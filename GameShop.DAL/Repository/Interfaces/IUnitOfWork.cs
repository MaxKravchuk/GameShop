using System.Threading.Tasks;
using GameShop.DAL.Entities;

namespace GameShop.DAL.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Comment> CommentRepository { get; }

        IRepository<Game> GameRepository { get; }

        IRepository<Genre> GenreRepository { get; }

        IRepository<PlatformType> PlatformTypeRepository { get; }

        IRepository<Publisher> PublisherRepository { get; }

        IRepository<Order> OrderRepository { get; }

        IRepository<OrderDetails> OrderDetailsRepository { get; }

        IRepository<User> UserRepository { get; }

        IRepository<UserTokens> UserTokensRepository { get; }

        IRepository<Role> RoleRepository { get; }

        Task SaveAsync();
    }
}
