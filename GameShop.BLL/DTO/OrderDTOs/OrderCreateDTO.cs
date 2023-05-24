using System;

namespace GameShop.BLL.DTO.OrderDTOs
{
    public class OrderCreateDTO
    {
        public int CustomerID { get; set; }

        public DateTime OrderedAt { get; set; }

        public string Strategy { get; set; }
    }
}
