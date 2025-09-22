using Core.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace ClassroomApi.Infrastructure.Presentation.Controllers
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

        [HttpPost("registro")]
        [AllowAnonymous]
        public async Task<IActionResult> Registro([FromBody] RegistroDTO registroDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _authService.RegistroAsync(registroDto);
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
            var token = await _authService.LoginAsync(loginDto);
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
            await _authService.ForgotPasswordAsync(forgotPasswordDto);
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

            var success = await _authService.ResetPasswordAsync(resetPasswordDto);
            if (!success)
            {
                return BadRequest(new { message = "El token es inválido o ha expirado." });
            }

            return Ok(new { message = "Contraseña restablecida correctamente." });
        }
    }
}
