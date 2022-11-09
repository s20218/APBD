using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Tutorial9.DTO;
using Tutorial9.DTOs.Requests;
using Tutorial9.Services;

namespace Tutorial9.Controllers
{
    [Authorize]
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ITokenHandlerService _tokenService;

        private readonly ILoginDbService _loginService;
            

        public AccountsController(ITokenHandlerService tokenService, ILoginDbService loginService)
        {
            _tokenService = tokenService;
            _loginService = loginService;
            
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var refreshToken = Guid.NewGuid();

            var result = await _loginService.LogUser(loginDTO, refreshToken.ToString());

            if (!result)
            {
                return StatusCode(400, "Incorrect user name or password provided");
            }
        

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(_tokenService.GenerateToken(loginDTO.UserName)),
                refreshToken = refreshToken
            });
        }



        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginDTO data)
        {
            var newRefreshToken = Guid.NewGuid();

            var result = await _loginService.AddUserToDbAsync(data, newRefreshToken.ToString());

            if (!result)
            {
                return StatusCode(400, "User name is used");
            }

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(_tokenService.GenerateToken(data.UserName)),
                refreshToken = newRefreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokensDTO tokens)
        {
            var userName = await _loginService.VerifyUserByToken(tokens);

            if(userName == "")
            {
                return StatusCode(404, "User is not found or token is not valid");
            }

            var newRefreshToken = _loginService.UpdateRefreshToken(tokens.RefreshToken, userName);

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(_tokenService.GenerateToken(userName)),
                refreshToken = newRefreshToken
            });
        }

    }
}
