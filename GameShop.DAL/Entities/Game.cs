using System;
using System.Collections.Generic;

namespace GameShop.DAL.Entities
{
    public class Game : BaseEntity
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Genre> GameGenres { get; set; } = new List<Genre>();

        public ICollection<PlatformType> GamePlatformTypes { get; set; } = new List<PlatformType>();

        public Publisher Publisher { get; set; }

        public int PublisherId { get; set; }

        public ICollection<OrderDetails> ListOfOrderDetails { get; set; } = new List<OrderDetails>();
    }
}
