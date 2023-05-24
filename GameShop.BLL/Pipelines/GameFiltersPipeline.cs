using System.Collections.Generic;
using System.Linq;
using GameShop.BLL.Filters.Interfaces;
using GameShop.BLL.Pipelines.Interfaces;
using GameShop.DAL.Entities;

namespace GameShop.BLL.Pipelines
{
    public class GameFiltersPipeline : IPipelinebase<IQueryable<Game>>
    {
        private IEnumerable<IOperation<IQueryable<Game>>> _operations;

        public IQueryable<Game> PerformOperation(IQueryable<Game> input)
        {
            return _operations.Aggregate(input, (current, operation) => operation.Execute(current));
        }

        public void Register(IEnumerable<IOperation<IQueryable<Game>>> operations)
        {
            _operations = operations;
        }
    }
}
