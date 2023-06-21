namespace GameShop.DAL.Entities
{
    public class Comment : BaseEntity
    {
        public string Name { get; set; }

        public string Body { get; set; }

        public bool HasQuotation { get; set; }

        public Game Game { get; set; }

        public int GameId { get; set; }

        public Comment Parent { get; set; }

        public int? ParentId { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }
    }
}
