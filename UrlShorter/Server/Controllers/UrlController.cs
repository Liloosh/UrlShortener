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

            if (result == Enums.UrlEnum.FullUrlIsExist)
            {
                return BadRequest("This full url is already exist!");
            }

            return Ok();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(AuthenticationSchemes = "Bearer", Policy = "UrlRequirements")]
        public async Task<IActionResult> DeleteShortUrl([FromRoute] int id)
        {
            var result = await _urlService.DeleteShortUrl(id);

            if (result == Enums.UrlEnum.SomethingWrong)
            {
                return BadRequest("Something went wrong!");
            }

            return Ok();
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
                return BadRequest("Url is not Exist!");
            }

            return Ok(result.Url);
        }
    }
}
