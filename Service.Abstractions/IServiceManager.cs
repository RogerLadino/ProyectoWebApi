using Core.Services.Abstractions;
using Persistence.Repositories;

namespace Service.Abstractions;
public interface IServiceManager
{
    IExerciseService ExerciseService { get; }
    IClassroomService ClassroomService { get; }
    IAuthService AuthService { get; }
    IEmailService EmailService { get; }
    ISubmissionService SubmissionService { get; }
    ICodeService CodeService { get; }
    ITokenBlacklistService TokenBlacklistService { get; }
}
