using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCats.Infrastructure;
using WebCats.Model;
using WebCats.ViewModels;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace WebCats.Controllers
{
    [ApiController]
    [Route("api/new/[controller]")]
    public class ImageUploadController : ControllerBase
    {
        private readonly ImageRepository _imageRepository;
        private static IWebHostEnvironment _environment;
        public ImageUploadController(IWebHostEnvironment environment, ImageRepository imageRepository)
        {
            _environment = environment;
            _imageRepository = imageRepository;
        }
        /*public class FIleUploadAPI
        {
            public IFormFile files { get; set; }
        }*/
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Create([FromForm] CreateImageViewModel imageViewModel)
        {            
            if (imageViewModel.Files.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + @"\uploads\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + @"\uploads\");
                    }
                    using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + @"\uploads\" + imageViewModel.Files.FileName))
                    {
                        imageViewModel.Files.CopyTo(filestream);
                        filestream.Flush();
                        var image = new Image(Guid.NewGuid(), imageViewModel.Files.FileName, imageViewModel.ResponseCode, DateTime.Now, _environment.WebRootPath + @"\uploads\" + imageViewModel.Files.FileName);
                        await _imageRepository.Create(image);
                        return Ok();
                    }
                }
                catch (Exception ex)
                {
                    return RedirectPreserveMethod(_environment.WebRootPath + @"\api\404");
                }
            }
            else
            {
                return BadRequest();
            }

        } 
    }
}