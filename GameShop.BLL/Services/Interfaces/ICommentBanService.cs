﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Services.Interfaces
{
    public interface ICommentBanService
    {
        void Ban(string banDuration);
    }
}
