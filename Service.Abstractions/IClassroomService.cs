using Shared.DTOs.Classroom;

namespace Service.Abstractions;

public interface IClassroomService : IServiceBase<ClassroomDto>
{
    Task<ClassroomDto> GetByCodeAsync(string code);
    Task<ClassroomDto> CreateAsync(ClassroomCreationDto classroomForCreationDto);
}