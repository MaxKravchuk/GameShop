namespace DAL.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using GameShop.DAL.Context;
    using GameShop.DAL.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<GameShopContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
    }
}
