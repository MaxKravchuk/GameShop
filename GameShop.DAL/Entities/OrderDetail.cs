namespace GameShop.DAL.Entities
{
    public class OrderDetail : BaseEntity
    {
        public Game Game { get; set; }

        public int GameId { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public Order Order { get; set; }

        public int OrderId { get; set; }
    }
}
