﻿namespace GameShop.BLL.DTO.PaymentDTOs
{
    public class PaymentCreateDTO
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public string Strategy { get; set; }
    }
}
