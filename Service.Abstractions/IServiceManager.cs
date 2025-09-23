namespace Service.Abstractions;
public interface IServiceManager
{
    IExerciseService ExerciseService { get; }
    IClassroomService ClassroomService { get; }
}
