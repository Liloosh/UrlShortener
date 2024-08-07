using Server.Models;

namespace Server.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task StoreRefreshToken(RefreshToken token);
        Task UpdateRefreshToken(RefreshToken token);
        Task<bool> RefreshTokenIsExist(string userId);
        Task<RefreshToken> GetRefreshToken(string userId);
        Task<bool> RefreshTokenIsExistByRefreshToken(string refreshToken);
        Task<RefreshToken> GetRefreshTokenByToken(string refreshToken);
    }
}
