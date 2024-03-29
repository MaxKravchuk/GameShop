﻿using GameShop.BLL.Services.Interfaces.Utils;
using log4net;

namespace GameShop.BLL.Services.Utils
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILog _logger;

        public LoggerManager(ILog logger)
        {
            _logger = logger;
        }

        public void LogDebug(string message) => _logger.Debug(message);

        public void LogError(string message) => _logger.Error(message);

        public void LogInfo(string message) => _logger.Info(message);
    }
}
