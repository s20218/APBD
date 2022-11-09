using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Tutorial9.Services
{
    public interface ITokenHandlerService
    {
        byte[] GenerateSalt();
        JwtSecurityToken GenerateToken(string userName);
    }

    public class TokenHandlerService : ITokenHandlerService
    {

        private readonly IConfiguration _configuration;

        public TokenHandlerService(IConfiguration configuration)
        {

            _configuration = configuration;
        }

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[128];

            using(var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(salt);
            }

            return salt;
        }

        public JwtSecurityToken GenerateToken(string userName)
        {

            Claim[] claims =
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, "user"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "http://localhost:44378",
                audience: "http://localhost:44378",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
                );

            return token;
        }
    }
}
