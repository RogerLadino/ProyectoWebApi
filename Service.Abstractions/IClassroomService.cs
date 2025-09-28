using Shared.DTOs.Classroom;

namespace Service.Abstractions;

public interface IClassroomService
{
    Task<ClassroomDto> GetByCodeAsync(string code);
    Task<ClassroomDto> CreateAsync(ClassroomCreationDto classroomForCreationDto);
    Task UpdateAsync(int classroomId, ClassroomUpdateDto classroomUpdateDto);
    Task<IEnumerable<ClassroomDto>> GetAllAsync();
    Task<ClassroomDto> GetByIdAsync(int id);
    Task DeleteAsync(int id);
    Task<IEnumerable<ClassroomDto>> GetByUserIdAsync(string userId); // Cambiado el tipo de retorno
}