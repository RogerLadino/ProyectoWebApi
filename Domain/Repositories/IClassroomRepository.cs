using Domain.Entities;

namespace Domain.Repositories;

public interface IClassroomRepository : IRepositoryBase<Classroom>
{
    Task<Classroom?> GetByIdWithUsersAsync(int classroomId);
    Task<Classroom?> GetByCodeAsync(string code);
    Task<bool> IsUserInClassroomAsync(string userId, int classroomId);
    Task<Classroom?> GetByIdWithUsersAndRolesAsync(int classroomId);
    Task<IEnumerable<Classroom>> GetByUserIdWithUsersAndRolesAsync(string userId);
}