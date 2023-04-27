using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.RedisDTOs
{
    public class CartItem
    {
        public string GameKey { get; set; }

        public string GameName { get; set; }

        public decimal GamePrice { get; set; }

        public short Quantity { get; set; } = 1;
    }
}
