using System.Collections.Generic;

namespace GameShop.DAL.Entities
{
    public class PlatformType : BaseEntity
    {
        public string Type { get; set; }

        public ICollection<Game> GamePlatformTypes { get; set; } = new List<Game>();
    }
}
