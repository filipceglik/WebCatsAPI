using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebCats.Helpers;
using WebCats.Infrastructure;
using WebCats.Model;
using WebCats.ViewModels;

namespace WebCats.Controllers
{
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly AppSettings _appSettings;

        public UserController(UserRepository userRepository, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("api/user/new")]
        public async Task<ActionResult> Create([FromBody] CreateUserViewModel createUserViewModel)
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword(createUserViewModel.Password);
            var user = new User(Guid.NewGuid(),createUserViewModel.UserName,hashed,DateTime.Now,createUserViewModel.Role);
            await _userRepository.Create(user);
            return Ok();
        }
        
        [AllowAnonymous]
        [HttpPost("api/user/login")]
        public async Task<ActionResult> Login([FromForm] LoginViewModel loginViewModel)
        {
            var user = await _userRepository.GetUser(loginViewModel.UserName);

            if (user is null)
            {
                return BadRequest();
            }
            if (!BCrypt.Net.BCrypt.Verify(loginViewModel.Password,user.Password))
            {
                return BadRequest();
            }

            

            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, loginViewModel.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                }),
                Expires = DateTime.UtcNow.AddSeconds(_appSettings.LifetimeInSeconds),
                SigningCredentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = _appSettings.ValidIssuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
//            user.Token = tokenHandler.WriteToken(token);

            return Ok
            (
                new {access_token = tokenHandler.WriteToken(token)}
            );
        }
    }
}