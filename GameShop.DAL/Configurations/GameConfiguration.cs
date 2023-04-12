using GameShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.DAL.Configurations
{
    public class GameConfiguration : EntityTypeConfiguration<Game>
    {
        public GameConfiguration()
        {
            ToTable("Games");

            HasKey(x => x.Id);
            
            Property(x => x.Key)
               .HasColumnType("varchar")
               .HasMaxLength(255)
               .HasColumnAnnotation(
                "Index", new IndexAnnotation(new[]
                {
                    new IndexAttribute("Index") { IsUnique = true }
                }))
                .IsRequired();

            Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            Property(x => x.Description)
                .HasMaxLength(255);

            HasMany<Genre>(game => game.GameGenres)
                .WithMany(genre => genre.GameGenres);

            HasMany<PlatformType>(game=>game.GamePlatformTypes)
                .WithMany(platformType=>platformType.GamePlatformTypes);
        }
    }
}
