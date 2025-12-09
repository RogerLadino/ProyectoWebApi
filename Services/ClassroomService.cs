using Domain.Entities;
using Domain.Exceptions.Classroom;
using Domain.Repositories;
using Mapster;
using Service.Abstractions;
using Shared.DTOs.Classroom;

namespace Services;

public class ClassroomService : IClassroomService
{
    private readonly IRepositoryManager _repositoryManager;
    private const string ClassroomNotFoundMessage = "No classroom exists with the given ID";

    public ClassroomService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<IEnumerable<ClassroomDto>> GetByUserIdAsync(string userId)
    {
        var user = await _repositoryManager.UsuarioRepository.GetByIdWithClassroomsAsync(userId);
        if (user == null)
            return Enumerable.Empty<ClassroomDto>();

        return user.Classrooms.Adapt<IEnumerable<ClassroomDto>>();
    }

    public async Task<ClassroomDto> CreateAndAssignProfessorAsync(ClassroomCreationDto classroomForCreationDto, string userId)
    {
        var classroomDto = await CreateAsync(classroomForCreationDto);
        var classroom = await _repositoryManager.ClassroomRepository.GetByIdWithUsersAsync(classroomDto.Id);
        var user = await _repositoryManager.UsuarioRepository.GetByIdAsync(userId);

        if (classroom != null && user != null)
        {
            classroom.AppUsers.Add(user);
            await _repositoryManager.SaveChangesAsync();
        }

        return classroomDto;
    }

    public async Task JoinClassroomByCodeAsync(string code, string userId)
    {
        var classroom = await _repositoryManager.ClassroomRepository.GetByCodeAsync(code);
        if (classroom == null)
            throw new ClassroomNotFoundException("No se encontró un aula con ese código");

        var user = await _repositoryManager.UsuarioRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException("Usuario no encontrado");

        var isAlreadyMember = await _repositoryManager.ClassroomRepository.IsUserInClassroomAsync(userId, classroom.Id);
        if (isAlreadyMember)
            throw new InvalidOperationException("Ya estás unido a este aula");

        classroom.AppUsers.Add(user);
        await _repositoryManager.SaveChangesAsync();
    }

    public async Task<IEnumerable<ClassroomDto>> GetAllAsync()
    {
        var classrooms = await _repositoryManager.ClassroomRepository.GetAllAsync();
        return classrooms.Adapt<IEnumerable<ClassroomDto>>();
    }

    public async Task<ClassroomDto> GetByIdAsync(int id)
    {
        var classroom = await _repositoryManager.ClassroomRepository.GetByIdAsync(id);
        if (classroom is null)
            throw new ClassroomNotFoundException(ClassroomNotFoundMessage);

        return classroom.Adapt<ClassroomDto>();
    }

    public async Task<ClassroomDto> CreateAsync(ClassroomCreationDto classroomForCreationDto)
    {
        var classroom = classroomForCreationDto.Adapt<Classroom>();
        classroom.Code = await GenerateUniqueCodeAsync();

        _repositoryManager.ClassroomRepository.Add(classroom);
        await _repositoryManager.SaveChangesAsync();

        return classroom.Adapt<ClassroomDto>();
    }

    public async Task UpdateAsync(int classroomId, ClassroomUpdateDto classroomUpdateDto)
    {
        var classroom = await _repositoryManager.ClassroomRepository.GetByIdAsync(classroomId);
        if (classroom is null)
            throw new ClassroomNotFoundException(ClassroomNotFoundMessage);

        classroom.Name = classroomUpdateDto.Name;

        var classroomExists = await _repositoryManager.ClassroomRepository
            .AnyAsync(e => e.Name.Equals(classroom.Name) && e.Id != classroomId);

        if (classroomExists)
            throw new ClassroomAlreadyExistsException("A classroom with the same name already exists");

        await _repositoryManager.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var classroom = await _repositoryManager.ClassroomRepository.GetByIdWithUsersAsync(id);
        if (classroom is null)
            throw new ClassroomNotFoundException(ClassroomNotFoundMessage);

        classroom.AppUsers.Clear();
        _repositoryManager.ClassroomRepository.Remove(classroom);
        await _repositoryManager.SaveChangesAsync();
    }

    public async Task<ClassroomDto> GetByCodeAsync(string code)
    {
        var classroom = await _repositoryManager.ClassroomRepository.GetByCodeAsync(code);
        if (classroom is null)
            throw new ClassroomNotFoundException("No classroom exists with the given code");

        return classroom.Adapt<ClassroomDto>();
    }

    public async Task<ClassroomWithTeacherDto> GetByIdWithTeacherAsync(int id)
    {
        var classroom = await _repositoryManager.ClassroomRepository.GetByIdWithUsersAndRolesAsync(id);
        if (classroom is null)
            throw new ClassroomNotFoundException(ClassroomNotFoundMessage);

        return MapToClassroomWithTeacher(classroom);
    }

    public async Task<IEnumerable<ClassroomWithTeacherDto>> GetByUserIdWithTeacherAsync(string userId)
    {
        var classrooms = await _repositoryManager.ClassroomRepository.GetByUserIdWithUsersAndRolesAsync(userId);
        return classrooms.Select(MapToClassroomWithTeacher);
    }

    private async Task<string> GenerateUniqueCodeAsync()
    {
        string code;
        bool exists;

        do
        {
            code = GenerateRandomCode(6);
            exists = await _repositoryManager.ClassroomRepository.AnyAsync(c => c.Code == code);
        }
        while (exists);

        return code;
    }

    private static string GenerateRandomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private ClassroomWithTeacherDto MapToClassroomWithTeacher(Classroom classroom)
    {
        var teacher = classroom.AppUsers
            .FirstOrDefault(u => u.AppRole?.Description?.ToLower() == "profesor" || u.AppRoleId == 1);

        return new ClassroomWithTeacherDto
        {
            Id = classroom.Id,
            Name = classroom.Name,
            Code = classroom.Code,
            Teacher = teacher != null ? new TeacherInfoDto
            {
                UserId = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Email = teacher.Email
            } : null
        };
    }
}
