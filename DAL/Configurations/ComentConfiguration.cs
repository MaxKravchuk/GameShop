using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class ComentConfiguration : EntityTypeConfiguration<Coment>
    {
        public ComentConfiguration()
        {
            this.ToTable("Coments");

            this.HasKey(x => x.Id);

            this
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            this
                .Property(x => x.Body)
                .HasMaxLength(255)
                .IsRequired();

            this
                .HasOptional<Game>(g => g.Game)
                .WithMany(g => g.Coments)
                .HasForeignKey<string>(g => g.GameKey);
        }
    }
}
