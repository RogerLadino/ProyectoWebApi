using Domain.Repositories;
using Service.Abstractions;
using Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IExerciseService> _lazyExerciseService;
    private readonly Lazy<IClassroomService> _lazyClassroomService;
    public ServiceManager(IRepositoryManager repositoryManager)
    {
        _lazyExerciseService = new Lazy<IExerciseService>(() => new ExerciseService(repositoryManager));
        _lazyClassroomService = new Lazy<IClassroomService>(() => new ClassroomService(repositoryManager));
    }

    public IExerciseService ExerciseService => _lazyExerciseService.Value;
    public IClassroomService ClassroomService => _lazyClassroomService.Value;
}
