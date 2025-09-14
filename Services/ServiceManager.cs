using Domain.Repositories;
using Service.Abstractions;
using Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IExerciseService> _lazyExerciseService;

    public ServiceManager(IRepositoryManager repositoryManager)
    {
        _lazyExerciseService = new Lazy<IExerciseService>(() => new ExerciseService(repositoryManager));
    }

    public IExerciseService ExerciseService => _lazyExerciseService.Value;
}
