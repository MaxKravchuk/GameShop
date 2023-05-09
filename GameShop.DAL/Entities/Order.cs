using System;
using System.Collections.Generic;

namespace GameShop.DAL.Entities
{
    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }

        public DateTime OrderedAt { get; set; }

        public ICollection<OrderDetails> ListOfOrderDetails { get; set; } = new List<OrderDetails>();
    }
}
