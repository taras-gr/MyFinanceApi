using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyFinance.Repositories.Interfaces;
using MyFinance.Services.Helpers;
using MyFinance.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyFinance.Services
{
    public class AuthenticationManagerService : IAuthenticationManagerService
    {
        private string _secretKey;
        private IUserRepository _userRepository;

        public AuthenticationManagerService(IOptions<JwtSettings> jwtSettings, IUserRepository userRepository)
        {
            _secretKey = jwtSettings.Value.JwtSecret;
            _userRepository = userRepository;
        }

        public async System.Threading.Tasks.Task<(string, string)> AuthenticateAsync(string email, string password)
        {
            var userFromRepo = await _userRepository.GetUserByEmail(email);
            var secretKey = GetSecretKeyAsByteArray();

            if (userFromRepo == null || userFromRepo.Password != password)
            {
                return (null, null);
            }
               
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", userFromRepo.Id.ToString()),
                    new Claim("UserName", userFromRepo.UserName.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return (token, userFromRepo.UserName);            
        }

        private byte[] GetSecretKeyAsByteArray()
        {
            if (_secretKey == null)
            {
                throw new ArgumentNullException(message: "JWT secret key is null", null);
            }

            var key = Encoding.UTF8.GetBytes(_secretKey);

            return key;
        }
    }
}