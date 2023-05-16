using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Filters.Interfaces
{
    public interface IOperation<T>
    {
        T Execute(T input);
    }
}
