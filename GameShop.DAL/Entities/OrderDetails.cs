namespace GameShop.DAL.Entities
{
    public class OrderDetails : BaseEntity
    {
        public Game Game { get; set; }

        public int GameId { get; set; }

        public short Quantity { get; set; }

        public float Discount { get; set; }

        public Order Order { get; set; }

        public int OrderId { get; set; }
    }
}
