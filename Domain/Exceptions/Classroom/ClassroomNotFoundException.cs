namespace Domain.Exceptions.Classroom;

public class ClassroomNotFoundException : Exception
{
    public ClassroomNotFoundException(string message) : base(message) { }
}
