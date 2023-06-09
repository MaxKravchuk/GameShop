using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDetailDTOs;
using GameShop.DAL.Entities;

namespace GameShop.BLL.DTO.OrderDTOs
{
    public class OrderReadDTO : OrderReadListDTO
    {
        public IEnumerable<OrderDetailsReadListDTO> OrderDetails { get; set; } = new List<OrderDetailsReadListDTO>();
    }
}
