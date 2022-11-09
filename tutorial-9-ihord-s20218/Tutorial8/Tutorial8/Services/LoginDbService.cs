using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tutorial9.DTO;
using Tutorial9.DTOs.Requests;
using Tutorial9.Models;

namespace Tutorial9.Services
{
    public interface ILoginDbService
    {
        Task<bool> LogUser(LoginDTO login, string refreshToken);
        Task<bool> AddUserToDbAsync(LoginDTO login, string refreshToken);
        Task<string> VerifyUserByToken(TokensDTO tokens);
        Task<Guid> UpdateRefreshToken(string refreshToken, string userName);
    }

    public class LoginDbService: ILoginDbService
    {
        private readonly MedsDbContext _context;
        private readonly ITokenHandlerService _tokenService;       

        public LoginDbService(MedsDbContext context, ITokenHandlerService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> AddUserToDbAsync(LoginDTO login, string refreshToken)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserName == login.UserName);

            if (userExists)
            {
                return false;
            }

            var salt = _tokenService.GenerateSalt();

            var user = new UserTb
            {
                UserName = login.UserName,
                Password = HashPassword(login.Password, salt),
                Salt = salt,
                RefreshedToken = refreshToken,
                ExpirationDate = DateTime.Now.AddDays(1)
            };

            _context.Users.Add(user);

            _context.SaveChanges();

            return true;

        }

        public async Task<bool> LogUser(LoginDTO login, string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == login.UserName);
        
            if(user is null)
            {
                return false;
            }                      

            var salt = user.Salt;
            var passedPassword = login.Password;

            var hashedPassword = HashPassword(passedPassword, salt);

            if(hashedPassword != user.Password)
            {
                return false;
            }

            user.RefreshedToken = refreshToken;

            _context.SaveChanges();

            return true;
        }

        public async Task<string> VerifyUserByToken(TokensDTO tokens)
        {
            var jwt = tokens.AccessToken;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var userName = token.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;

            var userExists = await _context.Users.AnyAsync(u => u.UserName == userName && u.RefreshedToken == tokens.RefreshToken
                                                           && u.ExpirationDate < DateTime.Now);

            if (!userExists)
            {
                return "";
            }

            return userName;
        }

        public async Task<Guid> UpdateRefreshToken(string refreshToken, string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName && u.RefreshedToken == refreshToken);

            var newRefreshToken = Guid.NewGuid();

            user.RefreshedToken = newRefreshToken.ToString();

            _context.SaveChanges();

            return newRefreshToken;

        }

        private string HashPassword(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));

            return hashed;
        }
    }
}
