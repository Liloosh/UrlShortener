using Microsoft.Extensions.Caching.Memory;
using Server.Dtos;
using Server.Enums;
using Server.Models;
using Server.Repositories.IRepositories;
using Server.Services.Caching;
using Server.Services.IServices;
using System.Data;
using System.Security.Cryptography;

namespace Server.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IRedisCaching _redisCaching;
        private readonly IMemoryCache _memoryCache;

        public UrlService(IUrlRepository urlRepository, IRedisCaching redisCaching, IMemoryCache memoryCache)
        {
            _urlRepository = urlRepository;
            _redisCaching = redisCaching;
            _memoryCache = memoryCache;
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
            var data = _memoryCache.TryGetValue("urls", out IEnumerable<Url> urls);

            IEnumerable<Url> result;

            if (!data)
            {
                result = await _urlRepository.GetUrlsAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                    SlidingExpiration = TimeSpan.FromSeconds(30)
                };

                _memoryCache.Set("urls", result, cacheOptions);

                return result.ToList();
            }
            //var data = _redisCaching.GetData<IEnumerable<Url>>("urls");

            //if(data is not null)
            //{
            //    return data.ToList();
            //}

            //result = await _urlRepository.GetUrlsAsync();

            //_redisCaching.SetData<IEnumerable<Url>>("urls", result);

            return urls.ToList();
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

        public async Task<UrlResponseDto> GetUrlByShortUrl(string shortUrl)
        {
            var result = await _urlRepository.GetUrlByShortUrl(shortUrl);

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
