using Domain.Repositories;

namespace Persistence.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryDbContext _dbContext;

        private readonly Lazy<IExerciseRepository> _lazyExerciseRepository;
        private readonly Lazy<IClassroomRepository> _lazyClassroomRepository;

        public RepositoryManager(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;

            _lazyExerciseRepository = new Lazy<IExerciseRepository>(() => new ExerciseRepository(_dbContext));
            
            _lazyClassroomRepository = new Lazy<IClassroomRepository>(() => new ClassroomRepository(_dbContext));
        }
        public IClassroomRepository ClassroomRepository => _lazyClassroomRepository.Value;   

        public IExerciseRepository ExerciseRepository =>  _lazyExerciseRepository.Value;
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}