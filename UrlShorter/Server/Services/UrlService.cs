using Server.Dtos;
using Server.Enums;
using Server.Models;
using Server.Repositories.IRepositories;
using Server.Services.IServices;
using System.Security.Cryptography;

namespace Server.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;

        public UrlService(IUrlRepository urlRepository)
        {
            _urlRepository = urlRepository;
        }

        public async Task<UrlEnum> CreateShortUrl(UrlDto dto)
        {
            string trimedFullUrl = dto.FullUrl.Trim();
            bool isExist = await _urlRepository.IsExistFullUrl(trimedFullUrl);
            if (isExist)
            {
                return UrlEnum.FullUrlIsExist;
            }

            string shortUrl = String.Empty;

            do
            {
                shortUrl = Convert.ToBase64String(RandomNumberGenerator.GetBytes(10));
                if (await _urlRepository.IsExistShortUrl(shortUrl))
                {
                    isExist = true;
                }
            } while (isExist);

            await _urlRepository.CreateShortUrl(dto, shortUrl);
            return UrlEnum.Ok;
        }

        public async Task<UrlEnum> DeleteShortUrl(int id)
        {
            var isExist = await _urlRepository.IsExistById(id);

            if (!isExist)
            {
                return UrlEnum.SomethingWrong;
            }

            await _urlRepository.DeleteShortUrl(id);

            return UrlEnum.Ok;
        }

        public async Task<List<Url>> GetAllUrls()
        {
            var result = await _urlRepository.GetUrlsAsync();

            return result;
        }

        public async Task<UrlResponseDto> GetUrlById(int id)
        {
            var result = await _urlRepository.GetUrlById(id);

            if (result == null)
            {
                return new UrlResponseDto() 
                {
                    Response = UrlEnum.UrlIsNotExit,
                };
            }

            return new UrlResponseDto()
            {
                Response = UrlEnum.Ok,
                Url = result
            };
        }
    }
}
