using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Dtos;
using Server.Models;
using Server.Repositories.IRepositories;

namespace Server.Repositories
{
    public class UrlRepository: IUrlRepository
    {
        private readonly DataContext _context;

        public UrlRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> IsExistFullUrl(string fullUrl)
        {
            bool isExist = await _context.Urls.AnyAsync(x => x.FullUrl == fullUrl);
            return isExist;
        }

        public async Task<bool> IsExistShortUrl(string shortUrl)
        {
            var isExist = await _context.Urls.AnyAsync(x => x.ShortUrl == shortUrl);
            return isExist;
        }

        public async Task<bool> IsExistById(int id)
        {
            var isExist = await _context.Urls.AnyAsync(x => x.Id == id);
            return isExist;
        }

        public async Task CreateShortUrl(UrlDto dto, string shortUrl)
        {
            var url = new Url()
            {
                FullUrl = dto.FullUrl,
                ShortUrl = shortUrl,
                CreatedDate = DateTime.UtcNow,
                UserId = dto.UserId,
            };

            await _context.Urls.AddAsync(url);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShortUrl(int id)
        {
            var url = await _context.Urls.SingleOrDefaultAsync(x => x.Id == id);
            _context.Urls.Remove(url!);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Url>> GetUrlsAsync()
        {
            var result = await _context.Urls.ToListAsync();

            return result;
        }

        public async Task<Url?> GetUrlById(int id)
        {
            var result = await _context.Urls.SingleOrDefaultAsync(x => x.Id == id);

            return result;
        }
    }
}
