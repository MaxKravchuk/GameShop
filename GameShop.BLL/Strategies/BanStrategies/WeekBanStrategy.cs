using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Strategies.BanStrategies
{
    public class WeekBanStrategy : IBanStrategy
    {
        public User Ban(User user)
        {
            user.BannedTo = DateTime.UtcNow.AddDays(7);
            return user;
        }
    }
}
