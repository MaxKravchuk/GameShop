using GameShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DAL.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Comment> CommentRepository { get; }

        IRepository<Game> GameRepository { get; }

        IRepository<Genre> GenreRepository { get; }

        IRepository<PlatformType> PlatformTypeRepository { get; }

        IRepository<Publisher> PublisherRepository { get; }

        Task SaveAsync();
    }
}
