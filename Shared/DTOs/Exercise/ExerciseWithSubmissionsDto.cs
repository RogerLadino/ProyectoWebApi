using Shared.DTOs.Submission;

namespace Shared.DTOs.Exercise;

public class ExerciseWithSubmissionsDto
{
    public int Id { get; set; }

    public int ClassroomId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? DueDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public ICollection<SubmissionDto> Submissions { get; set; } = new List<SubmissionDto>();
}
