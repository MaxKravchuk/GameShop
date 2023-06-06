using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.OrderDTOs
{
    public class OrderReadListDTO
    {
        public int Id { get; set; }

        public string CustomerNickName { get; set; }

        public DateTime OrderedAt { get; set; }

        public string Status { get; set; }
    }
}
