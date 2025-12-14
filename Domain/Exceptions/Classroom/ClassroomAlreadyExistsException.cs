namespace Domain.Exceptions.Classroom
{
    public class ClassroomAlreadyExistsException : Exception
    {
        public ClassroomAlreadyExistsException(string message) : base(message) { }
    }
}