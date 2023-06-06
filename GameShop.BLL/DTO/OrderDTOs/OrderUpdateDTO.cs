using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDetails;

namespace GameShop.BLL.DTO.OrderDTOs
{
    public class OrderUpdateDTO
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public IEnumerable<OrderDetailsUpdateDTO> OrderDetails { get; set; } = new List<OrderDetailsUpdateDTO>();
    }
}
