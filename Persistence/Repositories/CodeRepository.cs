using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class CodeRepository : RepositoryBase<Code>, ICodeRepository 
{
    public CodeRepository(RepositoryDbContext repositoryContext) : base(repositoryContext)
    {
    }
}
