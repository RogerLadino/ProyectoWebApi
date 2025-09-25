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

        //  Generar automáticamente un código único
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

        // Solo actualizamos el nombre
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

    public Task<ClassroomDto> GetByCodeAsync(string code)
    {
        throw new NotImplementedException();
    }

    // MÉTODOS PRIVADOS
    private async Task<string> GenerateUniqueCodeAsync()
    {
        string code;
        bool exists;

        do
        {
            code = GenerateRandomCode(6); // 6 caracteres aleatorios
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
