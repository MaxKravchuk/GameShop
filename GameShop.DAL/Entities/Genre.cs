using System.Collections.Generic;

namespace GameShop.DAL.Entities
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }

        public int? ParentGenreId { get; set; }

        public Genre ParentGenre { get; set; }

        public ICollection<Genre> SubGenres { get; set; } = new List<Genre>();

        public ICollection<Game> GameGenres { get; set; } = new List<Game>();
    }
}
