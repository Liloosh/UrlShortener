using Server.Dtos;
using Server.Models;

namespace Server.Repositories.IRepositories
{
    public interface IUrlRepository
    {
        Task CreateShortUrl(UrlDto dto, string shortUrl);
        Task<bool> IsExistShortUrl(string shortUrl);
        Task<bool> IsExistFullUrl(string fullUrl);
        Task<bool> IsExistById(int id);
        Task DeleteShortUrl(int id);
        Task<List<Url>> GetUrlsAsync();
        Task<Url?> GetUrlById(int id);
    }
}
