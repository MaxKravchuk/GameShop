using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.DAL.Entities;

namespace GameShop.DAL.Configurations
{
    public class PublisherConfiguration : EntityTypeConfiguration<Publisher>
    {
        public PublisherConfiguration()
        {
            ToTable("Publishers");

            HasKey(x => x.Id);

            Property(x => x.CompanyName)
                .HasColumnType("nvarchar")
                .HasMaxLength(40)
                .IsRequired();

            Property(x => x.Description)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            Property(x => x.HomePage)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            HasMany<Game>(x => x.Games)
                .WithRequired(x => x.Publisher)
                .HasForeignKey(x => x.PublisherId);
        }
    }
}
