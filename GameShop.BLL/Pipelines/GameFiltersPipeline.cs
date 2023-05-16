using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Filters.Interfaces;
using GameShop.BLL.Pipelines.Interfaces;
using GameShop.DAL.Entities;
using Unity;

namespace GameShop.BLL.Pipelines
{
    public class GameFiltersPipeline : IPipelinebase<IEnumerable<Game>>
    {
        private IEnumerable<IOperation<IEnumerable<Game>>> _operations;

        public IEnumerable<Game> PerformOperation(IEnumerable<Game> input)
        {
            return _operations.Aggregate(input, (current, operation) => operation.Execute(current));
        }

        public void Register(IEnumerable<IOperation<IEnumerable<Game>>> operations)
        {
            _operations = operations;
        }
    }
}
