using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;

namespace GameShop.BLL.Services
{
    public class CommentBanService : ICommentBanService
    {
        private readonly ILoggerManager _loggerManager;

        public CommentBanService(ILoggerManager loggerManager)
        {
            _loggerManager = loggerManager;
        }

        public void Ban(string banDuration)
        {
            _loggerManager.LogInfo("Service invoked successfully");
        }
    }
}
