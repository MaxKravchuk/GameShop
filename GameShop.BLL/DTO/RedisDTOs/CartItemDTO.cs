namespace GameShop.BLL.DTO.RedisDTOs
{
    public class CartItemDTO
    {
        public int CustomerId { get; set; }

        public string GameKey { get; set; }

        public string GameName { get; set; }

        public decimal GamePrice { get; set; }

        public short Quantity { get; set; } = 1;
    }
}
