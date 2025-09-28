using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class ClassroomRepository : RepositoryBase<Classroom>, IClassroomRepository
    {
        private readonly RepositoryDbContext _context;

        public ClassroomRepository(RepositoryDbContext repositoryContext) : base(repositoryContext)
        {
            _context = repositoryContext;
        }

        public async Task<Classroom?> GetByIdWithUsersAsync(int classroomId)
        {
            return await _context.Classrooms
                .Include(c => c.AppUsers) // Asegúrate de que la relación exista en tu entidad
                .FirstOrDefaultAsync(c => c.Id == classroomId);
        }

        public async Task<Classroom?> GetByCodeAsync(string code)
        {
            return await _context.Classrooms
                .Include(c => c.AppUsers)
                .FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<bool> IsUserInClassroomAsync(string userId, int classroomId)
        {
            // Convertir userId a int para comparar correctamente con AppUser.Id
            if (!int.TryParse(userId, out int userIdInt))
                return false;

            return await _context.Classrooms
                .Where(c => c.Id == classroomId)
                .AnyAsync(c => c.AppUsers.Any(u => u.Id == userIdInt));
        }
    }
}
