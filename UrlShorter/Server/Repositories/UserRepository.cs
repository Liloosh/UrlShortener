using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;
using Server.Repositories.IRepositories;

namespace Server.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context) 
        {
            _context = context;
        }

        public async Task StoreRefreshToken(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRefreshToken(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RefreshTokenIsExist(string userId)
        {
            var result = await _context.RefreshTokens.AnyAsync(x => x.UserId == userId);
            return result;
        }

        public async Task<RefreshToken> GetRefreshToken(string userId)
        {
            var refreshToken = await _context.RefreshTokens.Include(x => x.User).SingleOrDefaultAsync(x => x.UserId == userId);
            return refreshToken!;
        }

        public async Task<bool> RefreshTokenIsExistByRefreshToken(string refreshToken)
        {
            var result = await _context.RefreshTokens.AnyAsync(x => x.Token == refreshToken);
            return result;
        }

        public async Task<RefreshToken> GetRefreshTokenByToken(string refreshToken)
        {
            var token = await _context.RefreshTokens.Include(x => x.User).SingleOrDefaultAsync(x => x.Token == refreshToken);
            return token!;
        }
    }
}
