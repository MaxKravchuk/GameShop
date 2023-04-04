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
            .HasMaxLength(50);

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

            //this
            //    .HasMany(gen => gen.GameGenres)
            //    .WithMany(g => g.GameGenres)
            //    .Map(gg =>
            //    {
            //        gg.MapLeftKey("GameRefKey");
            //        gg.MapRightKey("GenreRefName");
            //        gg.ToTable("GameGenre");

            //    });

            //this
            //    .HasMany<PlatformType>(pt => pt.GamePlatformTypes)
            //    .WithMany(g => g.GamePlatformTypes)
            //    .Map(gg =>
            //    {
            //        gg.MapLeftKey("GameRefKey");
            //        gg.MapRightKey("PlatformTypeRefType");
            //        gg.ToTable("GamePlatformType");

            //    });
        }
    }
}
