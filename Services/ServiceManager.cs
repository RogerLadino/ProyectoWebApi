using Domain.Realtime;
using Domain.Repositories;
using Service.Abstractions;
using Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IExerciseService> _lazyExerciseService;

    private readonly Lazy<ISubmissionService> _lazySubmissionService;

    private readonly Lazy<ICodeService> _lazyCodeService;

    public ServiceManager(IRepositoryManager repositoryManager, IRealtimeManager realtimeManager)
    {
        _lazyExerciseService = new Lazy<IExerciseService>(() => new ExerciseService(repositoryManager));
        _lazySubmissionService = new Lazy<ISubmissionService>(() => new SubmissionService(repositoryManager));
        _lazyCodeService = new Lazy<ICodeService>(() => new CodeService(repositoryManager, realtimeManager));
    }

    public IExerciseService ExerciseService => _lazyExerciseService.Value;
    public ISubmissionService SubmissionService => _lazySubmissionService.Value;
    public ICodeService CodeService => _lazyCodeService.Value;
}
