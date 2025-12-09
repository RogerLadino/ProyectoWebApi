using Shared.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace Core.Services.Abstractions
{
    public interface IAuthService
    {
        Task<AppUserDto> Profile(int userId);
        Task<string?> LoginAsync(LoginDto loginDto);
        Task<bool> RegistroAsync(RegistroDto registroDto);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
