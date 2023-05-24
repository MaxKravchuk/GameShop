using System;
using System.Linq;
using GameShop.BLL.Enums;
using GameShop.BLL.Enums.Extensions;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Filters.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Filters
{
    public class CreatedAtFilter : IOperation<IQueryable<Game>>
    {
        private string _createdAtType;

        public IOperation<IQueryable<Game>> SetFilterDate(string createdAtType)
        {
            _createdAtType = createdAtType;
            return this;
        }

        public IQueryable<Game> Execute(IQueryable<Game> input)
        {
            return ApplyFilter(input);
        }

        private IQueryable<Game> ApplyFilter(IQueryable<Game> games)
        {
            if (string.IsNullOrEmpty(_createdAtType))
            {
                return games;
            }

            var createdAtType = _createdAtType.ToEnum<CreatedAtTypes>();
            switch (createdAtType)
            {
                case CreatedAtTypes.LastWeek:
                    DateTime lastWeek = DateTime.UtcNow.AddDays(-7);
                    games = games.Where(game => game.CreatedAt >= lastWeek);
                    break;
                case CreatedAtTypes.LastMonth:
                    DateTime lastMonth = DateTime.Now.AddMonths(-1);
                    games = games.Where(game => game.CreatedAt >= lastMonth);
                    break;
                case CreatedAtTypes.LastYear:
                    DateTime lastYear = DateTime.Now.AddYears(-1);
                    games = games.Where(game => game.CreatedAt >= lastYear);
                    break;
                case CreatedAtTypes.Last2Years:
                    DateTime last2Years = DateTime.Now.AddYears(-2);
                    games = games.Where(game => game.CreatedAt >= last2Years);
                    break;
                case CreatedAtTypes.Last3Years:
                    DateTime last3Years = DateTime.Now.AddYears(-3);
                    games = games.Where(game => game.CreatedAt >= last3Years);
                    break;
                default:
                    throw new BadRequestException("Incorrect filter parameter");
            }

            return games;
        }
    }
}
