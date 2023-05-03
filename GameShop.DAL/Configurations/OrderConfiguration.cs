using System.Data.Entity.ModelConfiguration;
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
