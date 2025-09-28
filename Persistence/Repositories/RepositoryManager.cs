using Domain.Repositories;

namespace Persistence.Repositories
{
    public sealed class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryDbContext _dbContext;

        private readonly Lazy<IExerciseRepository> _lazyExerciseRepository;
        private readonly Lazy<IClassroomRepository> _lazyClassroomRepository;
        private readonly Lazy<IUsuarioRepository> _lazyUsuarioRepository;
        private readonly Lazy<IUserClassroomRepository> _lazyUserClassroomRepository;

        public RepositoryManager(RepositoryDbContext dbContext)
        {
            _dbContext = dbContext;

            _lazyExerciseRepository = new Lazy<IExerciseRepository>(() => new ExerciseRepository(_dbContext));
            _lazyClassroomRepository = new Lazy<IClassroomRepository>(() => new ClassroomRepository(_dbContext));
            _lazyUsuarioRepository = new Lazy<IUsuarioRepository>(() => new UsuarioRepository(_dbContext));
            _lazyUserClassroomRepository = new Lazy<IUserClassroomRepository>(() => new UserClassroomRepository(_dbContext));
        }

        public IClassroomRepository ClassroomRepository => _lazyClassroomRepository.Value;
        public IExerciseRepository ExerciseRepository => _lazyExerciseRepository.Value;
        public IUsuarioRepository UsuarioRepository => _lazyUsuarioRepository.Value;
        public IUserClassroomRepository UserClassroomRepository => _lazyUserClassroomRepository.Value;

        object IRepositoryManager.UserRepository { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}