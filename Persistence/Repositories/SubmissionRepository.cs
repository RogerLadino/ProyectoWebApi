using Domain.Entities;
using Domain.Repositories;

namespace Persistence.Repositories;

public class SubmissionRepository : RepositoryBase<Submission>, ISubmissionRepository
{
    public SubmissionRepository(RepositoryDbContext repositoryContext) : base(repositoryContext)
    {
    }
}
