namespace GameShop.BLL.Services.Interfaces.Utils
{
    public interface ILoggerManager
    {
        void LogInfo(string message);

        void LogDebug(string message);

        void LogError(string message);
    }
}
