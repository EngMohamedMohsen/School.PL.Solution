namespace School.PL.Helper.Services
{
    public interface IRedisService
    {
        Task SetValueAsync(string key, string value, TimeSpan? expiry);
        Task<string?> GetValueAsync(string key);
        Task<bool> DeleteKeyAsync(string key);
    }
}
