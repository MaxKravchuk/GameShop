namespace DAL.Migrations
{
    using DAL.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.Context.GameShopContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DAL.Context.GameShopContext context)
        {
            List<Coment> dComents = new List<Coment>
            {
                new Coment {Id = 1, Name = "Admin", Body = "First comment",GameKey = "game1"},
                new Coment {Id = 2, Name = "Bob", Body = "[Admin]Reply to admin", GameKey = "game1"},
                new Coment {Id = 3, Name = "Bob", Body = "BlaBLa", GameKey = "game2"}
            };

            List<Genre> dGenres = new List<Genre>
            {
                new Genre {Id = 1, Name = "Test1", SubGenres = new List<Genre>
                {
                    new Genre {Id = 3, Name = "Test3"}
                }},
                new Genre {Id = 2, Name = "Test2"},
            };

            List<PlatformType> dPt = new List<PlatformType>
            {
                new PlatformType{Id = 1, Type = "Type1"},
                new PlatformType{Id = 2, Type = "Type2"}
            };

            List<Game> dGames = new List<Game>
            {
                new Game {Id = 1, Key = "game1", Name = "FirstGame", Description="Best game", Coments = new List<Coment>
                {
                    dComents[0],
                    dComents[1]
                }},
                new Game {Id = 2, Key = "game2", Name = "SecGame", Description="The Best game", Coments = new List<Coment>
                {
                    dComents[2]
                }},
            };

            List<GameGenre> gameGenres = new List<GameGenre>
            {
                new GameGenre{GameKey = "game1", Name = "Test1"},
                new GameGenre{GameKey="game2", Name="Test3"},
                new GameGenre{GameKey="game2", Name="Test2"},
            };

            List<GamePlatformType> gamePlatformTypes = new List<GamePlatformType>
            {
                new GamePlatformType{GameKey = "game1", Type = "Type1"},
                new GamePlatformType{GameKey = "game2", Type = "Type2"},
                new GamePlatformType{GameKey = "game2", Type = "Type1"},
            };

            context.Coments.AddRange(dComents);
            context.Genres.AddRange(dGenres);
            context.PlatformTypes.AddRange(dPt);
            context.Games.AddRange(dGames);
            context.GameGenres.AddRange(gameGenres);
            context.GamePlatformTypes.AddRange(gamePlatformTypes);
            context.SaveChanges();
        }
    }
}
