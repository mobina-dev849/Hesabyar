using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Shared.Dtos;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto, CancellationToken ct)
        {
            var errors = await _authService.Register(dto, ct);

            if (errors.Any())
            {
                return BadRequest(new
                {
                    message = "ثبت‌نام انجام نشد.",
                    errors
                });
            }

            return Ok(new
            {
                message = "ثبت‌نام با موفقیت انجام شد."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto, CancellationToken ct)
        {
            var token = await _authService.Login(dto, ct);

            if (token == null)
                return Unauthorized("شماره موبایل یا رمز عبور اشتباه است.");

            return Ok(new
            {
                token
            });
        }
    }
}