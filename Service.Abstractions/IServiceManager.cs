using Core.Services.Abstractions;
using Persistence.Repositories;

namespace Service.Abstractions;
public interface IServiceManager
{
    IExerciseService ExerciseService { get; }
    IAuthService AuthService { get; }
    IEmailService EmailService { get; }
}
