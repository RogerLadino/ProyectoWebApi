using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Persistence.Repositories
{
    public interface IUsuarioRepository : IRepositoryBase<AppUser>
    {
        Task AddAsync(AppUser nuevoUsuario);
        Task<AppUser?> GetByEmailAsync(string email);
        Task<AppUser?> GetByResetTokenAsync(string token);
        Task UpdateAsync(AppUser usuario);
    }
}