using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class CreatedAtFilter : IOperation<IEnumerable<Game>>
    {
        private string _dateOption;

        public IOperation<IEnumerable<Game>> SetFilterDate(string dateOption)
        {
            _dateOption = dateOption;
            return this;
        }

        public IEnumerable<Game> Execute(IEnumerable<Game> input)
        {
            return ApplyFilter(input);
        }

        private IEnumerable<Game> ApplyFilter(IEnumerable<Game> games)
        {
            if (!string.IsNullOrEmpty(_dateOption))
            {
                switch (_dateOption)
                {
                    case "lastWeek":
                        DateTime lastWeek = DateTime.UtcNow.AddDays(-7);
                        games = games.Where(game => game.CreatedAt >= lastWeek);
                        break;
                    case "lastMonth":
                        DateTime lastMonth = DateTime.Now.AddMonths(-1);
                        games = games.Where(game => game.CreatedAt >= lastMonth);
                        break;
                    case "lastYear":
                        DateTime lastYear = DateTime.Now.AddYears(-1);
                        games = games.Where(game => game.CreatedAt >= lastYear);
                        break;
                    case "last2Years":
                        DateTime last2Years = DateTime.Now.AddYears(-2);
                        games = games.Where(game => game.CreatedAt >= last2Years);
                        break;
                    case "last3Years":
                        DateTime last3Years = DateTime.Now.AddYears(-3);
                        games = games.Where(game => game.CreatedAt >= last3Years);
                        break;
                }
            }

            return games;
        }
    }
}
