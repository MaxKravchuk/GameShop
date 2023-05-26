using System.Data.Entity;
using DAL.Migrations;
using GameShop.DAL.Entities;

namespace GameShop.DAL.Context
{
    public class GameShopContext : DbContext
    {
        public GameShopContext() : base("name=DefaultConnectingString")
        {
            Database.SetInitializer(new GameShopInitializer());
        }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<PlatformType> PlatformTypes { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
        }
    }
}
