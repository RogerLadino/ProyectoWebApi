using Core.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using Shared.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Registro([FromBody] RegistroDTO registroDto)
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
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var token = await _serviceManager.AuthService.LoginAsync(loginDto);
            if (token == null)
            {
                return Unauthorized(new { message = "Credenciales incorrectas." });
            }

            return Ok(new LoginResponseDTO { Token = token });
        }

        [HttpPost("recuperar-clave")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPasswordDto)
        {
            await _serviceManager.AuthService.ForgotPasswordAsync(forgotPasswordDto);
            return Ok(new { message = "Si existe una cuenta con ese correo, un enlace de recuperación ha sido enviado." });
        }

        [HttpPost("nueva-clave")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDto)
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

        // Nuevo endpoint: POST api/auth/logout
        // No modifica el constructor: obtiene el servicio de revocación desde RequestServices (opcional).
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ")) return NoContent();

            var tokenString = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(tokenString)) return NoContent();

            var jwt = handler.ReadJwtToken(tokenString);
            var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrEmpty(jti)) return NoContent();

            DateTime expiresUtc = jwt.ValidTo == DateTime.MinValue ? DateTime.UtcNow : jwt.ValidTo;

            // Obtiene el servicio de revocación si está registrado; si no, no rompe la ejecución.
            var revocationServiceObj = HttpContext.RequestServices.GetService(typeof(ITokenRevocationService));
            if (revocationServiceObj is ITokenRevocationService revocationService)
            {
                revocationService.RevokeToken(jti, expiresUtc);
            }

            // El cliente debe eliminar también el token localmente (localStorage / cookie).
            return NoContent();
        }
    }
}
