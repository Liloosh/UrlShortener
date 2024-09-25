using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Server.Dtos;
using Server.Enums;
using Server.Models;
using Server.Repositories.IRepositories;
using Server.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Server.Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IUserRepository userRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<string> GenerateJwtToken(IdentityUser user)
        {
            var role = await _userManager.GetRolesAsync(user);

            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>()
            {
                new Claim("UserId", user.Id),
                new Claim("UserName", user.UserName!),
                new Claim("Email", user.Email!),
                new Claim(ClaimTypes.Role, role[0])
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }

        private async Task<string> GenerateRefreshToken(string userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var RefreshToken = new RefreshToken()
            {
                Token = token,
                UserId = userId,
                ExpiredDate = DateTime.UtcNow.AddDays(7),
            };

            if (!await _userRepository.RefreshTokenIsExist(userId))
            {
                await _userRepository.StoreRefreshToken(RefreshToken);
            }
            else
            {
                var oldRefreshToken = await _userRepository.GetRefreshToken(userId);
                oldRefreshToken.Token = RefreshToken.Token;
                oldRefreshToken.ExpiredDate = RefreshToken.ExpiredDate;
                await _userRepository.UpdateRefreshToken(oldRefreshToken);
            }

            return token;
        }

        public async Task<RefreshTokenDto?> RefreshToken(string refreshToken)
        {
            var isExist = await _userRepository.RefreshTokenIsExistByRefreshToken(refreshToken);

            if (!isExist)
            {
                return null;
            }

            var RefreshToken = await _userRepository.GetRefreshTokenByToken(refreshToken);

            if (RefreshToken.ExpiredDate < DateTime.UtcNow)
            {
                return null;
            }

            var token = await GenerateJwtToken(RefreshToken.User);
            var newRefreshToken = await GenerateRefreshToken(RefreshToken.User.Id);

            var response = new RefreshTokenDto() { RefreshToken = newRefreshToken, Token = token };
            return response;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return new LoginResponseDto { Response = LoginResponseEnum.EmailOrPasswordIsNotCorrect };
            }

            //Check if the user is locked out
            var isLockedOut = await _userManager.IsLockedOutAsync(user);

            if (isLockedOut)
            {
                return new LoginResponseDto
                {
                    Response = LoginResponseEnum.UserIsLockedOut
                };
            }

            var isValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isValid)
            {

                // increase the locked out 
                await _userManager.AccessFailedAsync(user);

                // check is the user is locked out
                var userStatus = await _userManager.IsLockedOutAsync(user);

                if (userStatus)
                {
                    return new LoginResponseDto
                    {
                        Response = LoginResponseEnum.UserIsLockedOut
                    };
                }

                return new LoginResponseDto
                {
                    Response = LoginResponseEnum.EmailOrPasswordIsNotCorrect
                };
            }

            await _userManager.ResetAccessFailedCountAsync(user);

            var token = await GenerateJwtToken(user);


            var refreshToken = String.Empty;

            refreshToken = await GenerateRefreshToken(user.Id);

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Response = LoginResponseEnum.Ok
            };
        }

        public async Task<RegisterResponseDto> Register(RegisterRequestDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user != null)
            {
                return new RegisterResponseDto() { Response = RegisterResponseEnum.UserNameIsExist };
            }
            user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null)
            {
                return new RegisterResponseDto() { Response = RegisterResponseEnum.EmailIsExist };
            }

            var newUser = new IdentityUser()
            {
                UserName = dto.UserName,
                Email = dto.Email,
            };
            var result = await _userManager.CreateAsync(newUser, dto.Password);
            Console.WriteLine(result);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Ordinary");
                return new RegisterResponseDto()
                {
                    Response = RegisterResponseEnum.Ok
                };
            }
            return new RegisterResponseDto() { Response = RegisterResponseEnum.Bad, Result = result };
        }
    }
}
