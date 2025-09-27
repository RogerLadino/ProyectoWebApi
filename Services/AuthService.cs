using Core.Services.Abstractions;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Abstractions;
using Shared.DTOs.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Exceptions;
using System.Text;


namespace Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(IRepositoryManager repositoryManager, IEmailService emailService, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(LoginDTO loginDto)
        {
            var usuario = await _repositoryManager.UsuarioRepository.GetByEmailAsync(loginDto.CorreoElectronico);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(loginDto.Clave, usuario.Password))
            {
                throw new InvalidCredentialsException("Correo o clave incorrectos.");
            }

            return GenerateJwtToken(usuario);
        }

        public async Task<bool> RegistroAsync(RegistroDTO registroDto)
        {
            var usuarioExistente = await _repositoryManager.UsuarioRepository.GetByEmailAsync(registroDto.CorreoElectronico);
            if (usuarioExistente != null)
            {
                throw new UserAlreadyExistsException("El correo electrónico ya está en uso.");
            }

            var claveHash = BCrypt.Net.BCrypt.HashPassword(registroDto.Clave);
            var nuevoUsuario = new AppUser
            {
                Email = registroDto.CorreoElectronico,
                FirstName = registroDto.PrimerNombre,
                MiddleName = registroDto.SegundoNombre,
                LastName = registroDto.PrimerApellido,
                SecondLastName = registroDto.SegundoApellido, 
                Password = claveHash,
                AppRoleId = registroDto.RolId
            };

            await _repositoryManager.UsuarioRepository.AddAsync(nuevoUsuario);
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDto)
        {
            var usuario = await _repositoryManager.UsuarioRepository.GetByEmailAsync(forgotPasswordDto.CorreoElectronico);
            if (usuario == null)
            {
                throw new InvalidTokenException("El token de recuperación es inválido o ha expirado.");
            }

            var token = Guid.NewGuid().ToString();
            usuario.TokenResetPassword = token;
            usuario.FechaExpiracionToken = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["PasswordRecoverySettings:TokenExpirationInMinutes"]!));
            await _repositoryManager.UsuarioRepository.UpdateAsync(usuario);

            var resetLink = $"https://tu-dominio-frontend.com/reset-password?token={token}";
            var emailBody = $"Para restablecer tu contraseña, haz clic en el siguiente enlace: <a href=\"{resetLink}\">Restablecer contraseña</a>";
            await _emailService.SendEmailAsync(usuario.Email, "Restablecer tu contraseña", emailBody);

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDTO resetPasswordDto)
        {
            var usuario = await _repositoryManager.UsuarioRepository.GetByResetTokenAsync(resetPasswordDto.Token);
            if (usuario == null || usuario.FechaExpiracionToken < DateTime.UtcNow)
            {
                return false;
            }

            usuario.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NuevaClave);
            usuario.TokenResetPassword = null;
            usuario.FechaExpiracionToken = null;
            await _repositoryManager.UsuarioRepository.UpdateAsync(usuario);

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