using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.CommentDTOs
{
    public class CommentCreateDTO : CommentBaseDTO
    {
        public string GameKey { get; set; }

        public int? ParentId { get; set; } = null;
    }
}
