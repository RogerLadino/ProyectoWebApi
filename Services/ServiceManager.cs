using Domain.Repositories;
using Service.Abstractions;
using Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IExerciseService> _lazyExerciseService;

    private readonly Lazy<ISubmissionService> _lazySubmissionService;

    public ServiceManager(IRepositoryManager repositoryManager)
    {
        _lazyExerciseService = new Lazy<IExerciseService>(() => new ExerciseService(repositoryManager));
        _lazySubmissionService = new Lazy<ISubmissionService>(() => new SubmissionService(repositoryManager));
    }

    public IExerciseService ExerciseService => _lazyExerciseService.Value;
    public ISubmissionService SubmissionService => _lazySubmissionService.Value;
}
