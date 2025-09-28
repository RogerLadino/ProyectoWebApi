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

    public ClassroomService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<IEnumerable<ClassroomDto>> GetByUserIdAsync(string userId)
    {
        // Obtener el usuario con sus aulas
        var user = await _repositoryManager.UsuarioRepository.GetByIdWithClassroomsAsync(userId);
        if (user == null)
            return Enumerable.Empty<ClassroomDto>();

        // Mapear las aulas del usuario a ClassroomDto
        return user.Classrooms.Adapt<IEnumerable<ClassroomDto>>();
    }

    public async Task<ClassroomDto> CreateAndAssignProfessorAsync(ClassroomCreationDto classroomForCreationDto, string userId)
    {
        // Crear el aula
        var classroomDto = await CreateAsync(classroomForCreationDto);

        // Obtener el aula recién creada con la relación
        var classroom = await _repositoryManager.ClassroomRepository.GetByIdWithUsersAsync(classroomDto.Id);

        // Obtener el usuario usando UsuarioRepository
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
        // Buscar aula por código
        var classroom = await _repositoryManager.ClassroomRepository.GetByCodeAsync(code);
        if (classroom == null)
            throw new ClassroomNotFoundException("No se encontró un aula con ese código");

        // Obtener el usuario usando UsuarioRepository
        var user = await _repositoryManager.UsuarioRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException("Usuario no encontrado");

        // Verificar si ya está unido
        var isAlreadyMember = await _repositoryManager.ClassroomRepository.IsUserInClassroomAsync(userId, classroom.Id);
        if (isAlreadyMember)
            throw new InvalidOperationException("Ya estás unido a este aula");

        // Agregar usuario al aula
        classroom.AppUsers.Add(user);
        await _repositoryManager.SaveChangesAsync();
    }

    public async Task<IEnumerable<ClassroomDto>> GetAllAsync()
    {
        var classrooms = await _repositoryManager.ClassroomRepository.GetAllAsync();
        return classrooms.Adapt<IEnumerable<ClassroomDto>>();
    }

    public async Task<ClassroomDto> GetByIdAsync(int classroomId)
    {
        var classroom = await _repositoryManager.ClassroomRepository.GetByIdAsync(classroomId);
        if (classroom is null)
            throw new ClassroomNotFoundException("No classroom exists with the given ID");

        return classroom.Adapt<ClassroomDto>();
    }

    public async Task<ClassroomDto> CreateAsync(ClassroomCreationDto classroomForCreationDto)
    {
        var classroom = classroomForCreationDto.Adapt<Classroom>();

        // Generar automáticamente un código único
        classroom.Code = await GenerateUniqueCodeAsync();

        var classroomExists = await _repositoryManager.ClassroomRepository
            .AnyAsync(e => e.Name.Equals(classroom.Name) && e.Id != classroom.Id);

        if (classroomExists)
            throw new ClassroomAlreadyExistsException("A classroom with the same name already exists");

        _repositoryManager.ClassroomRepository.Add(classroom);
        await _repositoryManager.SaveChangesAsync();

        return classroom.Adapt<ClassroomDto>();
    }

    public async Task UpdateAsync(int classroomId, ClassroomUpdateDto classroomForUpdateDto)
    {
        var classroom = await _repositoryManager.ClassroomRepository.GetByIdAsync(classroomId);
        if (classroom is null)
            throw new ClassroomNotFoundException("No classroom exists with the given ID");

        classroom.Name = classroomForUpdateDto.Name;

        var classroomExists = await _repositoryManager.ClassroomRepository
            .AnyAsync(e => e.Name.Equals(classroom.Name) && e.Id != classroomId);

        if (classroomExists)
            throw new ClassroomAlreadyExistsException("A classroom with the same name already exists");

        await _repositoryManager.SaveChangesAsync();
    }

    public async Task DeleteAsync(int classroomId)
    {
        var classroom = await _repositoryManager.ClassroomRepository.GetByIdAsync(classroomId);
        if (classroom is null)
            throw new ClassroomNotFoundException("No classroom exists with the given ID");

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

    // MÉTODOS PRIVADOS
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

    private string GenerateRandomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
