using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Server.Dtos;
using Server.Enums;
using Server.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Services
{
    public class UserService: IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
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

        public async Task<LoginResponseDto> Login(LoginRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return new LoginResponseDto { Response = LoginResponseEnum.EmailOrPasswordIsNotCorrect };
            }

            var isValid = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!isValid)
            {
                return new LoginResponseDto
                {
                    Response = LoginResponseEnum.EmailOrPasswordIsNotCorrect
                };
            }

            var token = await GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Token = token,
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
