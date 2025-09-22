using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface IUsuarioRepository
    {
        Task<AppUser?> GetByEmailAsync(string email);
        Task<AppUser?> GetByResetTokenAsync(string token);
        Task AddAsync(AppUser usuario);
        Task UpdateAsync(AppUser usuario);
    }
}