using Shared.DTOs.Exercise;
using Shared.DTOs.Submission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface ISubmissionService
    {
        Task<IEnumerable<SubmissionDto>> GetAllAsync(int exerciseId);

        Task<SubmissionDto> GetByIdAsync(int userId, int exerciseId);

        Task AssignGrade(int userId, int exerciseId, int grade);

        Task<SubmissionDto> CreateAsync(SubmissionCreationDto submissionCreationDto);

        Task<ExerciseWithSubmissionsDto[]> GetClassroomSubmissions(int classroomId);
    }
}
