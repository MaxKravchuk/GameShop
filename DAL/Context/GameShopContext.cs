using DAL.Entities;
using EntityFramework.Filters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Context
{
    public class GameShopContext : DbContext
    {
        public DbSet<Coment> Coments { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<PlatformType> PlatformTypes { get; set; }

        public GameShopContext() : base("name=DefaultConnectingString")
        {
            Database.SetInitializer<GameShopContext>(new DropCreateDatabaseIfModelChanges<GameShopContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
        }
    }
}
