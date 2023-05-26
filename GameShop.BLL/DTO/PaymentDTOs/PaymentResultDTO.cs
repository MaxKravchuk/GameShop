using System.IO;

namespace GameShop.BLL.DTO.StrategyDTOs
{
    public class PaymentResultDTO
    {
        public int OrderId { get; set; }

        public MemoryStream InvoiceMemoryStream { get; set; }

        public bool IsPaymentSuccessful { get; set; }
    }
}
