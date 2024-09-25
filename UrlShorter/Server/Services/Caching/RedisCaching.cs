using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Server.Services.Caching
{
    public class RedisCaching : IRedisCaching
    {
        private readonly IDistributedCache _cache;

        public RedisCaching(IDistributedCache cache)
        {
            _cache = cache;
        }

        public T? GetData<T>(string key)
        {
            var data = _cache.GetString(key);

            if (data == null)
            {
                return default(T?);
            }

            return JsonSerializer.Deserialize<T>(data);
        }

        public async void SetData<T>(string key, T value)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            };

            await _cache.SetStringAsync("urls", JsonSerializer.Serialize(value), options);
        }
    }
}
