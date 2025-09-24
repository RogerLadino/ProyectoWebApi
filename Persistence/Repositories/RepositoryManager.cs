using Domain.Repositories;
using System.Runtime.CompilerServices;

namespace Persistence.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryDbContext _dbContext;

        private readonly Lazy<IExerciseRepository> _lazyExerciseRepository;

        private readonly Lazy<ISubmissionRepository> _lazySubmissionRepository;

        private readonly Lazy<ICodeRepository> _lazyCodeRepository;

        public RepositoryManager(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;

            _lazyExerciseRepository = new Lazy<IExerciseRepository>(() => new ExerciseRepository(_dbContext));
            _lazySubmissionRepository = new Lazy<ISubmissionRepository>(() => new SubmissionRepository(_dbContext));
            _lazyCodeRepository = new Lazy<ICodeRepository>(() => new CodeRepository(_dbContext));
        }

        public IExerciseRepository ExerciseRepository =>  _lazyExerciseRepository.Value;

        public ISubmissionRepository SubmissionRepository => _lazySubmissionRepository.Value;

        public ICodeRepository CodeRepository => _lazyCodeRepository.Value;

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}