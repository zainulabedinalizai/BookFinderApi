using BookFinderApi.DTOs;
using BookFinderApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookFinderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(UserRegisterDto userRegisterDto)
        {
            try
            {
                var user = await _authService.Register(userRegisterDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login(UserLoginDto userLoginDto)
        {
            try
            {
                var user = await _authService.Login(userLoginDto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
