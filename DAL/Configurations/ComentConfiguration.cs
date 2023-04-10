using GameShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DAL.Configurations
{
    public class ComentConfiguration : EntityTypeConfiguration<Comment>
    {
        public ComentConfiguration()
        {
            ToTable("Coments");

            HasKey(x => x.Id);

            Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();
            
            Property(x => x.Body)
                .HasMaxLength(255)
                .IsRequired();
            
            HasRequired(x => x.Game)
                .WithMany(x => x.Coments)
                .HasForeignKey(x => x.GameId)
                .WillCascadeOnDelete(false);
        }
    }
}
