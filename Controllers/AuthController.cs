using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Auth;
using MovieTicketBooking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/register

        [HttpPost("register")]
        [AllowAnonymous]

        public async Task<IActionResult> Register(
            [FromBody] RegisterRequest request)
        {
            try
            {
                var response = await _authService.RegisterAsync(request);

                return Ok(new
                {
                    message = "Registration successful.",
                    data = response
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]

        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginAsync(request);

                return Ok(new
                {
                    message = "Login successful.",
                    data = response
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new
                {
                    message = ex.Message
                });
            }
        }
    }
}