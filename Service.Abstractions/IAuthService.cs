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
        Task<string?> LoginAsync(LoginDTO loginDto);
        Task<bool> RegistroAsync(RegistroDTO registroDto);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDTO resetPasswordDto);
    }
}
