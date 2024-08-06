using Microsoft.AspNetCore.Identity;
using Server.Dtos;
using Server.Enums;

namespace Server.Services.IServices
{
    public interface IUserService
    {
        Task<RegisterResponseDto> Register(RegisterRequestDto dto);
        Task<LoginResponseDto> Login(LoginRequestDto dto);
        Task<string> GenerateJwtToken(IdentityUser user);
    }
}
