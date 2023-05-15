using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public abstract class GameFilter
    {
        public abstract IEnumerable<Game> ApplyFilter(IEnumerable<Game> games);
    }
}
