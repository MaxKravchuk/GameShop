using GameShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DAL.Configurations
{
    public class GenreConfiguration : EntityTypeConfiguration<Genre>
    {
        public GenreConfiguration()
        {
            ToTable("Genres");

            HasKey(x => x.Id);

            Property(x => x.Name)
                .HasMaxLength(50)
                .HasColumnType("varchar")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Name") { IsUnique = true }));

            Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            HasOptional<Genre>(g => g.ParentGenre)
                .WithMany(g => g.SubGenres)
                .HasForeignKey(g => g.ParentGenreId);
        }
    }
}
