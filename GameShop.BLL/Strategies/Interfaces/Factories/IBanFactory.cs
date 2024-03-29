﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Enums;
using GameShop.BLL.Strategies.Interfaces.Strategies;

namespace GameShop.BLL.Strategies.Interfaces.Factories
{
    public interface IBanFactory
    {
        IBanStrategy GetBanStrategy(BanOptions banOptions);
    }
}
