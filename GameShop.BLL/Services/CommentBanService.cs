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
