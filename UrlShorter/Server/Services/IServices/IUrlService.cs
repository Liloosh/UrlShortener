using Server.Dtos;
using Server.Enums;
using Server.Models;

namespace Server.Services.IServices
{
    public interface IUrlService
    {
        Task<UrlEnum> CreateShortUrl(UrlDto dto);
        Task<UrlEnum> DeleteShortUrl(int id);
        Task<List<Url>> GetAllUrls();
        Task<UrlResponseDto> GetUrlById(int id);
        Task<UrlResponseDto> GetUrlByShortUrl(string shortUrl);
    }
}
