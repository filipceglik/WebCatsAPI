using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    [Route("api/new")]
    public class ImageUploadController : ControllerBase
    {
        private readonly ImageRepository _imageRepository;
        private static IWebHostEnvironment _environment;
        private bool validFile;
        

        private static readonly Dictionary<string, List<byte[]>> _fileSignature = 
            new Dictionary<string, List<byte[]>>
            {
                { ".jpeg", new List<byte[]>
                    {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                    }
                },
            };
        public ImageUploadController(IWebHostEnvironment environment, ImageRepository imageRepository)
        {
            _environment = environment;
            _imageRepository = imageRepository;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Create([FromForm] CreateImageViewModel imageViewModel)
        {
            if (imageViewModel.Files.Length > 0 && imageViewModel.Files.FileName.Length > 0)
            {
                
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + "uploads/"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "uploads/");
                    }
                    
                    using (var reader = new BinaryReader(imageViewModel.Files.OpenReadStream()))
                    {
                        var signatures = _fileSignature[Path.GetExtension(imageViewModel.Files.FileName).ToLowerInvariant()];
                        var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                        validFile = signatures.Any(signature => 
                            headerBytes.Take(signature.Length).SequenceEqual(signature));
                    }
                    
                    if (validFile)
                    {
                        using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "uploads/" + imageViewModel.Files.FileName))
                        {
                            await imageViewModel.Files.CopyToAsync(filestream);
                            var image = new Image(Guid.NewGuid(), imageViewModel.Files.FileName, imageViewModel.ResponseCode, DateTime.Now, _environment.WebRootPath + "uploads/" + imageViewModel.Files.FileName);
                            filestream.Flush();
                            await _imageRepository.Create(image);
                            return Ok();
                        }  
                    }

                    return BadRequest();

                }
                //TODO: Implement authentication schema
                catch (KeyNotFoundException keyNotFoundException)
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        } 
    }
}