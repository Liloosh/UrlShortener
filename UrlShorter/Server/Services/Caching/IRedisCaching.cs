namespace Server.Services.Caching
{
    public interface IRedisCaching
    {
        T? GetData<T>(string key);
        void SetData<T>(string key, T value);
    }
}
