using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class GenreConfiguration : EntityTypeConfiguration<Genre>
    {
        public GenreConfiguration()
        {
            this.ToTable("Genres");

            this.HasKey(x => x.Name);

            this
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            this
                .HasOptional(g => g.ParentGenre)
                .WithMany(g => g.SubGenres)
                .HasForeignKey<string>(g => g.ParentGenreName);
        }
    }
}
