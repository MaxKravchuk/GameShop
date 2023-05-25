using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.PaymentDTOs
{
    public class PaymentCreateDTO
    {
        public int OrderId { get; set; }

        public string Strategy { get; set; }
    }
}
