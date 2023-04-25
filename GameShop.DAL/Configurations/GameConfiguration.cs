using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using GameShop.DAL.Entities;

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

            Property(x => x.Price)
                .HasColumnType("money")
                .IsRequired();

            Property(x => x.UnitsInStock)
                .HasColumnType("smallint")
                .IsRequired();

            Property(x => x.Discontinued)
                .HasColumnType("bit")
                .IsRequired();

            HasMany<Genre>(game => game.GameGenres)
                .WithMany(genre => genre.GameGenres);

            HasMany<PlatformType>(game => game.GamePlatformTypes)
                .WithMany(platformType => platformType.GamePlatformTypes);
        }
    }
}
