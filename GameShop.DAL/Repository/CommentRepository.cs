using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using GameShop.DAL.Context;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.DAL.Repository
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly GameShopContext _context;

        public CommentRepository(GameShopContext context) : base(context)
        {
            _context = context;
        }

        public Comment GetById_SP(int id)
        {
            return _context.Comments.SqlQuery("EXEC GetById @CommentId", new SqlParameter("@CommentId", id))
                .FirstOrDefault();
        }
    }
}
