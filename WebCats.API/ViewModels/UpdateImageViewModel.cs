using Microsoft.AspNetCore.Http;

namespace WebCats.ViewModels
{
    public class UpdateImageViewModel
    {
        public string ResponseCode { get; set; }
        public IFormFile Files { get; set; }
    }
}