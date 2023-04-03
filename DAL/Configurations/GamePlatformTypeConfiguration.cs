using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Configurations
{
    public class GamePlatformTypeConfiguration : EntityTypeConfiguration<GamePlatformType>
    {
        public GamePlatformTypeConfiguration()
        {
            this.ToTable("GamePlatformType");
            this.HasKey(x => new { x.GameKey, x.Type });
            this
                .HasRequired<Game>(x => x.Game)
                .WithMany(x => x.GamePlatformTypes)
                .HasForeignKey(x => x.GameKey);
            this
                .HasRequired<PlatformType>(x => x.PlatformType)
                .WithMany(x => x.GamePlatformTypes)
                .HasForeignKey(x => x.Type);
        }
    }
}
