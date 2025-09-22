using Service.Abstractions;
using Core.Services.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Persistence.Repositories;


namespace Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IEmailService emailService, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(LoginDTO loginDto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.CorreoElectronico);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Clave, usuario.ClaveHash))
            {
                return null;
            }

            return GenerateJwtToken(usuario);
        }

        public async Task<bool> RegistroAsync(RegistroDTO registroDto)
        {
            var usuarioExistente = await _usuarioRepository.GetByEmailAsync(registroDto.CorreoElectronico);
            if (usuarioExistente != null)
            {
                return false;
            }

            var claveHash = BCrypt.Net.BCrypt.HashPassword(registroDto.Clave);
            var nuevoUsuario = new AppUser
            {
                Email = registroDto.CorreoElectronico,
                FirstName = registroDto.NombreUsuario,
                Password = claveHash,
                AppRoleId = registroDto.RolId
            };

            await _usuarioRepository.AddAsync(nuevoUsuario);
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDto)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(forgotPasswordDto.CorreoElectronico);
            if (usuario == null)
            {
                return true;
            }

            var token = Guid.NewGuid().ToString();
            usuario.TokenResetPassword = token;
            usuario.FechaExpiracionToken = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["PasswordRecoverySettings:TokenExpirationInMinutes"]!));
            await _usuarioRepository.UpdateAsync(usuario);

            var resetLink = $"https://tu-dominio-frontend.com/reset-password?token={token}";
            var emailBody = $"Para restablecer tu contraseña, haz clic en el siguiente enlace: <a href=\"{resetLink}\">Restablecer contraseña</a>";
            await _emailService.SendEmailAsync(usuario.CorreoElectronico, "Restablecer tu contraseña", emailBody);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO resetPasswordDto)
        {
            var usuario = await _usuarioRepository.GetByResetTokenAsync(resetPasswordDto.Token);
            if (usuario == null || usuario.FechaExpiracionToken < DateTime.UtcNow)
            {
                return false;
            }

            usuario.ClaveHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NuevaClave);
            usuario.TokenResetPassword = null;
            usuario.FechaExpiracionToken = null;
            await _usuarioRepository.UpdateAsync(usuario);

            return true;
        }

        private string GenerateJwtToken(AppUser usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.AppRole.Description)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:ExpirationInMinutes"]!)),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}