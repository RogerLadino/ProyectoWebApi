namespace Service.Abstractions;
public interface IServiceManager
{
    IExerciseService ExerciseService { get; }
    ISubmissionService SubmissionService { get; }
    ICodeService CodeService { get; }
}
