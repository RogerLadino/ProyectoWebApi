using Core.Services;
using Core.Services.Abstractions;
using Domain.Realtime;
using Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Service.Abstractions;
using Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IExerciseService> _lazyExerciseService;
    private readonly Lazy<IClassroomService> _lazyClassroomService;
    private readonly Lazy<ISubmissionService> _lazySubmissionService;
    private readonly Lazy<ICodeService> _lazyCodeService;
    private readonly Lazy<IEmailService> _lazyEmailService;
    private readonly Lazy<IAuthService> _lazyAuthService;
    private readonly Lazy<ITokenBlacklistService> _lazyTokenBlacklistService;

    public ServiceManager(IRepositoryManager repositoryManager, IRealtimeManager realtimeManager, IConfiguration configuration)
    {
        _lazyClassroomService = new Lazy<IClassroomService>(() => new ClassroomService(repositoryManager));
        _lazyExerciseService = new Lazy<IExerciseService>(() => new ExerciseService(repositoryManager));
        _lazyEmailService = new Lazy<IEmailService>(() => new EmailService(configuration));
        _lazyAuthService = new Lazy<IAuthService>(() => new AuthService(repositoryManager, _lazyEmailService.Value, configuration));
        _lazySubmissionService = new Lazy<ISubmissionService>(() => new SubmissionService(repositoryManager));
        _lazyCodeService = new Lazy<ICodeService>(() => new CodeService(repositoryManager, realtimeManager));
        _lazyTokenBlacklistService = new Lazy<ITokenBlacklistService>(() => new TokenBlacklistService());
    }

    public IEmailService EmailService => _lazyEmailService.Value;
    public IAuthService AuthService => _lazyAuthService.Value;
    public IExerciseService ExerciseService => _lazyExerciseService.Value;
    public ISubmissionService SubmissionService => _lazySubmissionService.Value;
    public ICodeService CodeService => _lazyCodeService.Value;
    public IClassroomService ClassroomService => _lazyClassroomService.Value;
    public ITokenBlacklistService TokenBlacklistService => _lazyTokenBlacklistService.Value;
}
