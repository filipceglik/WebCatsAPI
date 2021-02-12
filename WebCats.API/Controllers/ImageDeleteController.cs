using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebCats.Infrastructure;
using WebCats.Model;

namespace WebCats.Controllers
{
    [ApiController]
    [Route("api/response/delete")]
    public class ImageDeleteController : ControllerBase
    {
        private readonly ImageRepository _imageRepository;
        private static IWebHostEnvironment _environment;

        public ImageDeleteController(IWebHostEnvironment environment, ImageRepository imageRepository)
        {
            _environment = environment;
            _imageRepository = imageRepository;
        }
        
        [Authorize(Roles = Role.Admin)]
        [HttpGet("{responseCode:int}")]
        public async Task<ActionResult> GetResponse(int responseCode)
        {
            try
            {
                System.IO.File.Delete(_environment.WebRootPath + "uploads/" + responseCode + ".jpeg");
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}