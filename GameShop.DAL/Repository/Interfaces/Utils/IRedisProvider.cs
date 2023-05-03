using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameShop.DAL.Repository.Interfaces.Utils
{
    public interface IRedisProvider<T>
    {
        Task<T> GetValueAsync(string redisKey, string redisValue);

        Task<bool> SetValueToListAsync(string redisKey, string exRedisValue, T newRedisValue);

        Task<IEnumerable<T>> GetValuesAsync(string redisKey);

        Task<bool> DeleteItemFromListAsync(string redisKey, string redisValue);
    }
}
