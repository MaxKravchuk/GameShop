﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.StrategyDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Strategies.Interfaces.Strategies
{
    public interface IPaymentStrategy
    {
        PaymentResultDTO Pay(Order order);
    }
}