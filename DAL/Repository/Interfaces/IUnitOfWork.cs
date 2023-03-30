using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        Repository<Coment> ComentRepository { get; }
        Repository<Game> GameRepository { get; }
        Repository<Genre> GenreRepository { get; }
        Repository<PlatformType> PlatformTypeRepository { get; }
        Task SaveAsync();

    }
}
