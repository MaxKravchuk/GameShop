using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class GameGenreConfiguration : EntityTypeConfiguration<GameGenre>
    {
        public GameGenreConfiguration()
        {
            this.ToTable("GameGenre");
            this.HasKey(x => new { x.GameKey, x.Name });
            this
                .HasRequired<Game>(x => x.Game)
                .WithMany(x => x.GameGenres)
                .HasForeignKey(x => x.GameKey);
            this
                .HasRequired<Genre>(x => x.Genre)
                .WithMany(x => x.GameGenres)
                .HasForeignKey(x => x.Name);
        }
    }
}
