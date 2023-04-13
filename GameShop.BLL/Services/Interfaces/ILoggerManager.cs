using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Services.Interfaces
{
    public interface ILoggerManager
    {
        void LogInfo(string message);

        void LogDebug(string message);

        void LogError(string message);
    }
}
