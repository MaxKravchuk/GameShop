using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Strategies.BanStrategies
{
    public class HourBanStrategy : IBanStrategy
    {
        public User Ban(User user)
        {
            user.BannedTo = DateTime.UtcNow.AddHours(1);
            return user;
        }
    }
}
