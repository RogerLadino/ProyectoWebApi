using Domain.Entities;
using Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UsuarioRepository : RepositoryBase<AppUser>, IUsuarioRepository
    {
        private readonly RepositoryDbContext _context;

        public UsuarioRepository(RepositoryDbContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
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
        // NUEVO MÉTODO PARA TRAER USUARIO + AULAS
        public async Task<AppUser?> GetByIdWithClassroomsAsync(string userId)
        {
            // Convertir el userId (string) a int para comparar con AppUser.Id (int)
            if (!int.TryParse(userId, out int id))
                return null;

            return await _context.AppUsers
                .Include(u => u.Classrooms)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<AppUser> GetByIdAsync(string userId)
        {
            if (!int.TryParse(userId, out int id))
                throw new ArgumentException("El userId debe ser un número entero válido.", nameof(userId));

            var user = await _context.AppUsers.FindAsync(id);
            if (user == null)
                throw new InvalidOperationException($"No se encontró el usuario con Id {userId}.");

            return user;
        }
    }
}   