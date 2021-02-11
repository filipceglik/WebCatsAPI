using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCats.Infrastructure;
using WebCats.Model;
using WebCats.ViewModels;

namespace WebCats.Controllers
{
    [ApiController]
    [Route("api/update")]
    public class ImageUpdateController : ControllerBase
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
        public ImageUpdateController(IWebHostEnvironment environment, ImageRepository imageRepository)
        {
            _environment = environment;
            _imageRepository = imageRepository;
        }
        
        //[Authorize(Roles = "Admin")]
        [Authorize(Roles = Role.Admin)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Update([FromForm] UpdateImageViewModel updateImageViewModel)
        {
            
            if (updateImageViewModel.Files.Length > 0 && updateImageViewModel.Files.FileName.Length > 0)
            {
                    
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + "uploads/"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "uploads/");
                    }
                        
                    using (var reader = new BinaryReader(updateImageViewModel.Files.OpenReadStream()))
                    {
                        var signatures = _fileSignature[Path.GetExtension(updateImageViewModel.Files.FileName).ToLowerInvariant()];
                        var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

                        validFile = signatures.Any(signature => 
                            headerBytes.Take(signature.Length).SequenceEqual(signature));
                    }

                    if (!validFile) return BadRequest();
                    await using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "uploads/" + updateImageViewModel.Files.FileName))
                    {
                        await updateImageViewModel.Files.CopyToAsync(filestream);
                        filestream.Flush();
                        return Ok();
                    }

                }
                //TODO: Implement authentication schema
                catch (KeyNotFoundException)
                {
                    return BadRequest();
                }
            }
            

            return Ok();
        }
    }
}