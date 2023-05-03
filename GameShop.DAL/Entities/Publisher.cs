using System.Collections.Generic;

namespace GameShop.DAL.Entities
{
    public class Publisher : BaseEntity
    {
        public string CompanyName { get; set; }

        public string Description { get; set; }

        public string HomePage { get; set; }

        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
