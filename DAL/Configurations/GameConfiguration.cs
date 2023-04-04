using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class GameConfiguration : EntityTypeConfiguration<Game>
    {
        public GameConfiguration()
        {
            this.ToTable("Games");

            this.HasKey(x => x.Id);

            this.Property(x => x.Key)
            .HasMaxLength(50)
            .IsRequired();

            this
                .Property(x => x.Key)
                .HasMaxLength(50)
                .HasColumnType("varchar")
                .HasColumnAnnotation("IX_Key", new IndexAnnotation(new IndexAttribute("IX_Key") { IsUnique = true }));

            this
                .Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            this
                .Property(x => x.Description)
                .HasMaxLength(255);

            this
                .HasMany<Genre>(game => game.GameGenres)
                .WithMany(genre => genre.GameGenres);

            this
                .HasMany<PlatformType>(game=>game.GamePlatformTypes)
                .WithMany(platformType=>platformType.GamePlatformTypes);
        }
    }
}
