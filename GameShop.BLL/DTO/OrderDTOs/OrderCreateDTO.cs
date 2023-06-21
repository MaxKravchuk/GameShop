using System;

namespace GameShop.BLL.DTO.OrderDTOs
{
    public class OrderCreateDTO
    {
        public int CustomerId { get; set; }

        public DateTime OrderedAt { get; set; }
    }
}
