using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Migrations;
using GameShop.DAL.Entities;

namespace GameShop.DAL.Context
{
    public class GameShopInitializer : CreateDatabaseIfNotExists<GameShopContext>
    {
        protected override void Seed(GameShopContext context)
        {
            List<Comment> dComments = new List<Comment>
            {
                new Comment { Id = 1, Name = "Admin", Body = "First comment", GameId = 1 },
                new Comment { Id = 2, Name = "Bob", Body = "[Admin]Reply to admin", GameId = 1, ParentId = 1 },
                new Comment { Id = 3, Name = "Bob", Body = "BlaBLa", GameId = 2 },
                new Comment { Id = 3, Name = "Admin", Body = "[Bob]Hello", GameId = 2, ParentId = 2 }
            };

            List<Genre> dGenres = new List<Genre>
            {
                new Genre
                {
                    Id = 1, Name = "Strategy",
                    SubGenres = new List<Genre>
                    {
                        new Genre { Id = 8, Name = "RTS" },
                        new Genre { Id = 9, Name = "TBS" },
                    }
                },
                new Genre { Id = 2, Name = "RPG" },
                new Genre { Id = 3, Name = "Sports" },
                new Genre
                {
                    Id = 4, Name = "Races",
                    SubGenres = new List<Genre>
                    {
                        new Genre { Id = 10, Name = "Rally" },
                        new Genre { Id = 11, Name = "Arcade" },
                        new Genre { Id = 12, Name = "Formula" },
                        new Genre { Id = 13, Name = "Off-road" },
                    }
                },
                new Genre
                {
                    Id = 5, Name = "Action",
                    SubGenres = new List<Genre>
                    {
                        new Genre { Id = 14, Name = "FPS" },
                        new Genre { Id = 15, Name = "TPS" },
                        new Genre { Id = 16, Name = "Misc" },
                    }
                },
                new Genre { Id = 6, Name = "Adventure" },
                new Genre { Id = 7, Name = "Puzzle&Skill" },
            };

            List<PlatformType> dPt = new List<PlatformType>
            {
                new PlatformType { Id = 1, Type = "Mobile" },
                new PlatformType { Id = 2, Type = "Browser" },
                new PlatformType { Id = 3, Type = "Desktop" },
                new PlatformType { Id = 4, Type = "Console" },
            };

            List<Publisher> publishers = new List<Publisher>
            {
                new Publisher
                {
                    CompanyName = "TechPub",
                    Description = "A cutting-edge technology publisher that specializes in books and online resources" +
                    "related to artificial intelligence, machine learning, and data science.",
                    HomePage = "https://www.techpub.com"
                },
                new Publisher
                {
                    CompanyName = "CodeMasters",
                    Description = "A renowned publisher of programming and coding resources, providing high-quality " +
                    "books, tutorials, and online courses on programming languages, web development, and software engineering.",
                    HomePage = "https://www.codemasters.com"
                },
            };

            List<Game> games = new List<Game>();

            games.Add(new Game
            {
                Id = 1,
                Key = "halo5",
                Name = "Halo 5: Guardians",
                Description = "A first-person shooter game set in a sci-fi universe",
                GameGenres = new List<Genre> { dGenres[4], dGenres[4], dGenres[0].SubGenres.Where(x => x.Id == 14).SingleOrDefault() },
                GamePlatformTypes = new List<PlatformType> { dPt[3] },
                Publisher = publishers[0],
                Price = 10,
                UnitsInStock = 2,
                CreatedAt = new System.DateTime(2022, 3, 15)
            });

            games.Add(new Game
            {
                Id = 2,
                Key = "civilization6",
                Name = "Sid Meier's Civilization VI",
                Description = "A turn-based strategy game where you lead a civilization from ancient times to modern era",
                GameGenres = new List<Genre> { dGenres[0], dGenres[0].SubGenres.Where(x => x.Id == 8).SingleOrDefault() },
                GamePlatformTypes = new List<PlatformType> { dPt[2], dPt[3] },
                Publisher = publishers[0],
                Price = 8,
                UnitsInStock = 5,
                CreatedAt = new System.DateTime(2019, 5, 11)
            });

            games.Add(new Game
            {
                Id = 3,
                Key = "fifa22",
                Name = "FIFA 22",
                Description = "A soccer simulation game featuring licensed teams and players",
                GameGenres = new List<Genre> { dGenres[2] },
                GamePlatformTypes = new List<PlatformType> { dPt[0], dPt[2], dPt[3] },
                Publisher = publishers[0],
                Price = 15,
                UnitsInStock = 0,
                CreatedAt = new System.DateTime(2021, 9, 25)
            });

            games.Add(new Game
            {
                Id = 4,
                Key = "forza7",
                Name = "Forza Motorsport 7",
                Description = "A racing game featuring realistic driving physics and licensed cars",
                GameGenres = new List<Genre> { dGenres[3], dGenres[3].SubGenres.Where(x => x.Id == 12).SingleOrDefault() },
                GamePlatformTypes = new List<PlatformType> { dPt[3] },
                Publisher = publishers[1],
                Price = 25,
                UnitsInStock = 10,
                CreatedAt = new System.DateTime(2017, 10, 3)
            });

            games.Add(new Game
            {
                Id = 5,
                Key = "minecraft",
                Name = "Minecraft",
                Description = "A sandbox game where you can build and explore a blocky world",
                GameGenres = new List<Genre> { dGenres[6] },
                GamePlatformTypes = new List<PlatformType> { dPt[0], dPt[1], dPt[2], dPt[3] },
                Publisher = publishers[1],
                Price = 100,
                UnitsInStock = 1,
                CreatedAt = new System.DateTime(2011, 11, 18)
            });

            context.Comments.AddRange(dComments);
            context.Genres.AddRange(dGenres);
            context.PlatformTypes.AddRange(dPt);
            context.Games.AddRange(games);
            context.Publishers.AddRange(publishers);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
