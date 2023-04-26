using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DAL.Entities
{
    public class Order : BaseEntity
    {
        public int CustomerId { get; set; }

        public DateTime OrderedAt { get; set; }

        public ICollection<OrderDetails> ListOfOrderDetails { get; set; } = new List<OrderDetails>();
    }
}
