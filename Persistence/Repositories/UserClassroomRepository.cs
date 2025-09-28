using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UserClassroomRepository : RepositoryBase<Classroom>, IUserClassroomRepository
{
    public UserClassroomRepository(RepositoryDbContext repositoryContext) : base(repositoryContext)
    {
    }
}