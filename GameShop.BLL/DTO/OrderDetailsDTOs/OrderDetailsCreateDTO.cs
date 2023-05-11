using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.OrderDetailsDTOs
{
    public class OrderDetailsCreateDTO
    {
        public string GameKey { get; set; }

        public int Quantity { get; set; }
    }
}
