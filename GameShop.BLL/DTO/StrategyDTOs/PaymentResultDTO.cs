using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.StrategyDTOs
{
    public class PaymentResultDTO
    {
        public int OrderId { get; set; }

        public MemoryStream InvoiceMemoryStream { get; set; }
    }
}
