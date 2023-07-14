using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.DAL.Entities;

namespace GameShop.DAL.Repository.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Comment GetById_SP(int id);
    }
}
