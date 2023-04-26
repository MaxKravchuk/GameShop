using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.DAL.Entities;

namespace GameShop.DAL.Configurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            ToTable("Orders");

            HasKey(x => x.Id);

            Property(x => x.CustomerId)
                .IsRequired();

            Property(x => x.OrderedAt)
                .IsRequired();
        }
    }
}
