using Domain.Entities;
using Domain.Exceptions.Exceptions;
using Domain.Repositories;
using Mapster;
using Microsoft.IdentityModel.Tokens;
using Service.Abstractions;
using Shared.DTOs.Exercise;
using Shared.DTOs.Submission;
using Shared.DTOs.Users;

namespace Services;
public class SubmissionService : ISubmissionService
{
    private readonly IRepositoryManager _repositoryManager;

    public SubmissionService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<IEnumerable<SubmissionDto>> GetAllAsync(int exerciseId)
    {
        var exercise = await _repositoryManager.ExerciseRepository.GetByIdAsync(exerciseId);

        var submissions = await _repositoryManager.SubmissionRepository
            .FindByConditionAsync(s => s.ExerciseId == exerciseId);

        var classroom = await _repositoryManager.ClassroomRepository
            .GetByIdWithUsersAsync(exercise.ClassroomId);

        var submissionDict = submissions
            .ToDictionary(s => s.AppUserId, s => s.Adapt<SubmissionDto>());

        var result = classroom.AppUsers
            .Where(user => user.AppRoleId != 1)
            .Select(user =>
            {
                if (submissionDict.TryGetValue(user.Id, out var existingSubmission))
                {
                    existingSubmission.AppUser = user.Adapt<AppUserDto>();
                    return existingSubmission;
                }

                return new SubmissionDto
                {
                    AppUserId = user.Id,
                    ExerciseId = exerciseId,
                    Grade = 0,
                    Status = 0,
                    SubmittedAt = null,
                    AppUser = user.Adapt<AppUserDto>()
                };
            });

        return result;
    }

    public async Task<SubmissionDto> GetByIdAsync(int userId, int exerciseId)
    {

        var submission = (await _repositoryManager.SubmissionRepository
            .FindByConditionAsync(s => s.ExerciseId == exerciseId && s.AppUserId == userId))
            .FirstOrDefault();

        if (submission == null)
        {
            SubmissionCreationDto newSubmission = new SubmissionCreationDto
            {
                AppUserId = userId,
                ExerciseId = exerciseId,
                Grade = 0,
                Status = 0,
                SubmittedAt = DateTime.MaxValue
            };

            return await CreateAsync(newSubmission);
        }

        var user = await _repositoryManager.UsuarioRepository.GetByIdAsync(submission.AppUserId);

        if(user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        submission.AppUser = user;

        return submission.Adapt<SubmissionDto>();
    }

    public async Task AssignGrade(int userId, int exerciseId, int grade)
    {
        var submission = (await _repositoryManager.SubmissionRepository
            .FindByConditionAsync(s => s.ExerciseId == exerciseId && s.AppUserId == userId))
            .FirstOrDefault();

        if (submission != null)
        {
            submission.Grade = grade;

           _repositoryManager.SubmissionRepository.Update(submission);
            await _repositoryManager.SaveChangesAsync();
            return;
        }


        var submissionDto = new SubmissionCreationDto
        {
            AppUserId = userId,
            ExerciseId = exerciseId,
            Grade = grade,
            Status = 0,
            SubmittedAt = DateTime.MaxValue
        };

        await CreateAsync(submissionDto);
    }

    public async Task<SubmissionDto> CreateAsync(SubmissionCreationDto submissionForCreationDto)
    {
        var submission = submissionForCreationDto.Adapt<Submission>();

        var submissionExists = await _repositoryManager.SubmissionRepository
            .AnyAsync(s => s.ExerciseId == submission.ExerciseId
                        && s.AppUserId == submission.AppUserId);

        if (submissionExists)
            throw new SubmissionAlreadyExistsException("Submission already exists");


        _repositoryManager.SubmissionRepository.Add(submission);
        await _repositoryManager.SaveChangesAsync();

        return submission.Adapt<SubmissionDto>();
    }

    public async Task<ExerciseWithSubmissionsDto[]> GetClassroomSubmissions(int classroomId)
    {
        var classroom = await _repositoryManager.ClassroomRepository
            .GetByIdWithUsersAsync(classroomId);

        if (classroom == null)
            return Array.Empty<ExerciseWithSubmissionsDto>();

        var exercises = await _repositoryManager.ExerciseRepository
            .FindByConditionAsync(e => e.ClassroomId == classroomId);

        var result = new List<ExerciseWithSubmissionsDto>();

        foreach (var exercise in exercises)
        {
            var submissions = await _repositoryManager.SubmissionRepository
                .FindByConditionAsync(s => s.ExerciseId == exercise.Id);

            var submissionDict = submissions
                .ToDictionary(s => s.AppUserId, s => s.Adapt<SubmissionDto>());

            var exerciseSubmissions = classroom.AppUsers
                .Where(user => user.AppRoleId != 1)
                .Select(user =>
                {
                    if (submissionDict.TryGetValue(user.Id, out var existingSubmission))
                    {
                        existingSubmission.AppUser = user.Adapt<AppUserDto>();
                        return existingSubmission;
                    }

                    return new SubmissionDto
                    {
                        AppUserId = user.Id,
                        ExerciseId = exercise.Id,
                        Grade = 0,
                        Status = 0,
                        SubmittedAt = null,
                        AppUser = user.Adapt<AppUserDto>()
                    };
                })
                .ToList();

            var exerciseWithSubmissions = new ExerciseWithSubmissionsDto
            {
                Id = exercise.Id,
                ClassroomId = exercise.ClassroomId,
                Name = exercise.Name,
                Description = exercise.Description,
                DueDate = exercise.DueDate,
                CreatedAt = exercise.CreatedAt,
                Submissions = exerciseSubmissions
            };

            result.Add(exerciseWithSubmissions);
        }

        return [.. result];
    }
}
