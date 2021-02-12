using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebCats.Infrastructure;
using WebCats.Model;

namespace WebCats.Controllers
{
    [ApiController]
    [Route("api/response")]
    public class ImageFetchController : ControllerBase
    {
        private readonly ImageRepository _imageRepository;
        private static IWebHostEnvironment _environment;

        public ImageFetchController(IWebHostEnvironment environment, ImageRepository imageRepository)
        {
            _environment = environment;
            _imageRepository = imageRepository;
        }
        
        [Authorize]
        [HttpGet("{responseCode:int}")]
        public async Task<ActionResult<Image>> GetResponse(int responseCode)
        {
            var img = await _imageRepository.GetImage(responseCode);
            try
            {
                return File(await System.IO.File.ReadAllBytesAsync(_environment.WebRootPath + "uploads/" + img.Filename), "image/jpeg");
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }
    }
}