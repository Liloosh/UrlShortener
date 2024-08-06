using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Dtos;
using Server.Enums;
using Server.Services.IServices;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            var result = await _userService.Register(dto);
            if(result.Response == RegisterResponseEnum.UserNameIsExist)
            {
                return BadRequest("UserName is already exist!");
            }
            else if(result.Response == RegisterResponseEnum.EmailIsExist)
            {
                return BadRequest("Email is already exist!");
            }
            else if(result.Response == RegisterResponseEnum.Ok)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.Result!.Errors);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var result = await _userService.Login(dto);

            if (result.Response == LoginResponseEnum.EmailOrPasswordIsNotCorrect) 
            {
                return BadRequest("Email or Password is not correct");
            }

            return Ok(result);
        }
    }
}
