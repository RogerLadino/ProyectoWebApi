using Core.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.DTOs.Users;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ClassroomApi.Infrastructure.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize]
        [HttpGet("perfil")]
        public async Task<IActionResult> Perfil()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
                return Unauthorized("UserId not found or invalid");

            var appUser = await _serviceManager.AuthService.Profile(userId);

            return Ok(appUser);
        }

        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<IActionResult> Registro([FromBody] RegistroDto registroDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _serviceManager.AuthService.RegistroAsync(registroDto);
            if (!success)
            {
                return Conflict(new { message = "El usuario ya existe." });
            }

            return Ok(new { message = "Registro exitoso." });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _serviceManager.AuthService.LoginAsync(loginDto);
            if (token == null)
            {
                return Unauthorized(new { message = "Credenciales incorrectas." });
            }

            return Ok(new LoginResponseDto { Token = token });
        }

        [HttpPost("recuperar-clave")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            await _serviceManager.AuthService.ForgotPasswordAsync(forgotPasswordDto);
            return Ok(new { message = "Si existe una cuenta con ese correo, un enlace de recuperación ha sido enviado." });
        }

        [HttpPost("nueva-clave")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _serviceManager.AuthService.ResetPasswordAsync(resetPasswordDto);
            if (!success)
            {
                return BadRequest(new { message = "El token es inválido o ha expirado." });
            }

            return Ok(new { message = "Contraseña restablecida correctamente." });
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token no proporcionado." });

            _serviceManager.TokenBlacklistService.RevokeToken(token);

            return Ok(new { message = "Token revocado exitosamente." });
        }
    }
}
