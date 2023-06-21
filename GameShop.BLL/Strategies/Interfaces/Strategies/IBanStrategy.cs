using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Strategies.Interfaces.Strategies
{
    public interface IBanStrategy
    {
        User Ban(User user);
    }
}
