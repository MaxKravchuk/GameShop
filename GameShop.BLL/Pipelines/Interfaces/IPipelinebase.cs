using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Filters.Interfaces;

namespace GameShop.BLL.Pipelines.Interfaces
{
    public interface IPipelinebase<T>
    {
        void Register(IEnumerable<IOperation<T>> operations);

        T PerformOperation(T input);
    }
}
