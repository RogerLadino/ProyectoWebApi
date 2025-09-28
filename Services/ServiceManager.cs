using Core.Services;
using Core.Services.Abstractions;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
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
    private readonly Lazy<IEmailService> _lazyEmailService;
    private readonly Lazy<IAuthService> _lazyAuthService;

    public ServiceManager(IRepositoryManager repositoryManager, IConfiguration configuration)
    {
        _lazyExerciseService = new Lazy<IExerciseService>(() => new ExerciseService(repositoryManager));
        _lazyEmailService = new Lazy<IEmailService>(() => new EmailService(configuration));
        _lazyAuthService = new Lazy<IAuthService>(() => new AuthService(repositoryManager, _lazyEmailService.Value, configuration));
    }

    public IEmailService EmailService => _lazyEmailService.Value;

    public IAuthService AuthService => _lazyAuthService.Value;

    public IExerciseService ExerciseService => _lazyExerciseService.Value;
    public IClassroomService ClassroomService => _lazyClassroomService.Value;
}
