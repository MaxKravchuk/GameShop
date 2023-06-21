using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Strategies.BanStrategies
{
    public class MonthBanStrategy : IBanStrategy
    {
        public User Ban(User user)
        {
            user.BannedTo = DateTime.UtcNow.AddMonths(1);
            return user;
        }
    }
}
