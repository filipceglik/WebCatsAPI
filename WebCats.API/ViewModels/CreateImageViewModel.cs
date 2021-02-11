using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace WebCats.ViewModels
{
    public class CreateImageViewModel
    {
        public string ResponseCode { get; set; }
        public IFormFile Files { get; set; }
    }
}