using System.Collections.Generic;
using GameShop.BLL.Filters.Interfaces;

namespace GameShop.BLL.Pipelines.Interfaces
{
    public interface IPipelinebase<T>
    {
        void Register(IEnumerable<IOperation<T>> operations);

        T PerformOperation(T input);
    }
}
