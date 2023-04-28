using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.DAL.Repository.Interfaces.Utils;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace GameShop.DAL.Repository
{
    public class RedisProvider<T> : IRedisProvider<T>
    {
        private readonly IDatabase _database;

        public RedisProvider(ConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task<T> GetValueAsync(string redisKey, string redisValue)
        {
            var existingItem = await _database.HashGetAsync(redisKey, redisValue);
            if (existingItem.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(existingItem);
            }

            return default;
        }

        public async Task<IEnumerable<T>> GetValuesAsync(string redisKey)
        {
            var values = await _database.HashValuesAsync(redisKey);
            var tValues = values.Select(x => JsonConvert.DeserializeObject<T>(x.ToString()));
            return tValues;
        }

        public async Task<bool> SetValueToListAsync(string redisKey, string exRedisValue, T newRedisObject)
        {
            var newRedisValue = JsonConvert.SerializeObject(newRedisObject);
            return await _database.HashSetAsync(redisKey, exRedisValue, newRedisValue);
        }

        public async Task<bool> DeleteItemFromListAsync(string redisKey, string redisValue)
        {
            return await _database.HashDeleteAsync(redisKey, redisValue);
        }
    }
}
