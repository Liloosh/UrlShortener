using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Server.Dtos;
using Server.Services.IServices;
using System.Data;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewShortUrl([FromBody]UrlDto dto)
        {
            var result = await _urlService.CreateShortUrl(dto);

            var response = new UrlResponseDto()
            {
                Response = result
            };
            if (result == Enums.UrlEnum.FullUrlIsExist)
            {
                response.Message = "This full url is already exist!";
                return BadRequest(response);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "UrlRequirements")]
        public async Task<IActionResult> DeleteShortUrl([FromRoute] int id)
        {
            var result = await _urlService.DeleteShortUrl(id);

            var response = new UrlResponseDto()
            {
                Response = result
            };
            if (result == Enums.UrlEnum.SomethingWrong)
            {
                response.Message = "Something went wrong!";
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUrls()
        {
            var urls = await _urlService.GetAllUrls();
            return Ok(urls);
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Ordinary")]
        public async Task<IActionResult> GetUrlById([FromRoute] int id)
        {
            var result = await _urlService.GetUrlById(id);

            if(result.Response == Enums.UrlEnum.UrlIsNotExit)
            {
                result.Message = "Url is not Exist!";
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("{shortUrl}")]
        public async Task<IActionResult> GetUrlByShortUrl([FromRoute] string shortUrl)
        {
            var result = await _urlService.GetUrlByShortUrl(shortUrl);

            if (result.Response == Enums.UrlEnum.UrlIsNotExit)
            {
                result.Message = "Url is not Exist!";
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
