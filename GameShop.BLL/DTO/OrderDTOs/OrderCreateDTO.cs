using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDetailsDTOs;

namespace GameShop.BLL.DTO.OrderDTOs
{
    public class OrderCreateDTO
    {
        public int CustomerID { get; set; }

        public DateTime OrderedAt { get; set; }
    }
}
