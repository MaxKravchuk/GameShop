using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class GameShopContext : DbContext
    {
        DbSet<Coment> Coments { get; set; }
        DbSet<Game> Games { get; set; }
        DbSet<GameGenre> GameGenres { get; set; }
        DbSet<GamePlatformType> GamePlatformTypes { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<PlatformType> PlatformTypes { get; set; }

        public GameShopContext() : base("name=DefaultConnectingString")
        {
            Database.SetInitializer<GameShopContext>(new DataSeeder());
        }
    }
}
