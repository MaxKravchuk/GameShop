using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.OrderDetails
{
    public class OrderDetailsReadListDTO
    {
        public int OrderDetailsId { get; set; }

        public string GameKey { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }
    }
}
