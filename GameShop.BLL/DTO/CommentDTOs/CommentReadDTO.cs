using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.CommentDTOs
{
    public class CommentReadDTO : CommentBaseDTO
    {
        public int Id { get; set; }

        public int ParrentId { get; set; }
    }
}
