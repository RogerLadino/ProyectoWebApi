using Domain.Entities;
using Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly RepositoryDbContext _context;

        public UsuarioRepository(RepositoryDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.AppUsers
                .Include(u => u.AppRole)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AppUser?> GetByResetTokenAsync(string token)
        {
            return await _context.AppUsers
                .FirstOrDefaultAsync(u => u.TokenResetPassword == token);
        }

        public async Task AddAsync(AppUser usuario)
        {
            _context.AppUsers.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppUser usuario)
        {
            _context.AppUsers.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}