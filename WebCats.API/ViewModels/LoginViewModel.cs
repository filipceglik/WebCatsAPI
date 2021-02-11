using System.IdentityModel.Tokens.Jwt;

namespace WebCats.ViewModels
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}