using Domain.Repositories;

namespace Persistence.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryDbContext _dbContext;

        private readonly Lazy<IExerciseRepository> _lazyExerciseRepository;

        private readonly Lazy<IUsuarioRepository> _lazyUsuarioRepository;

        public RepositoryManager(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;

            _lazyExerciseRepository = new Lazy<IExerciseRepository>(() => new ExerciseRepository(_dbContext));
            _lazyUsuarioRepository = new Lazy<IUsuarioRepository>(() => new UsuarioRepository(_dbContext));
        }

        public IExerciseRepository ExerciseRepository =>  _lazyExerciseRepository.Value;

        public IUsuarioRepository UsuarioRepository => _lazyUsuarioRepository.Value;

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}