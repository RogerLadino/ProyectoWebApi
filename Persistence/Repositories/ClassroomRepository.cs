using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class ClassroomRepository : RepositoryBase<Classroom>, IClassroomRepository
{
    public ClassroomRepository(RepositoryDbContext repositorycontext) : base(repositorycontext)
    {
    }
}
