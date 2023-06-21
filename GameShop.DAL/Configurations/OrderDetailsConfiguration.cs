using System.Data.Entity.ModelConfiguration;
using GameShop.DAL.Entities;

namespace GameShop.DAL.Configurations
{
    public class OrderDetailsConfiguration : EntityTypeConfiguration<OrderDetail>
    {
        public OrderDetailsConfiguration()
        {
            ToTable("OrderDetails");

            HasKey(x => x.Id);

            Property(x => x.GameId)
                .IsRequired();

            Property(x => x.Quantity)
                .IsRequired();

            HasRequired<Game>(od => od.Game)
                .WithMany(g => g.ListOfOrderDetails)
                .HasForeignKey(od => od.GameId);

            HasRequired<Order>(od => od.Order)
                .WithMany(o => o.ListOfOrderDetails)
                .HasForeignKey(od => od.OrderId);
        }
    }
}
