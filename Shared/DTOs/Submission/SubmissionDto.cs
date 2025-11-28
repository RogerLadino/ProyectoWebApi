using Shared.DTOs.Exercise;
using Shared.DTOs.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Submission;

public partial class SubmissionDto
{
    [Key]
    public int AppUserId { get; set; }

    [Key]
    public int ExerciseId { get; set; }

    public int Grade { get; set; }

    public byte Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SubmittedAt { get; set; }

    [ForeignKey("AppUserId")]
    [InverseProperty("Submissions")]
    public virtual AppUserDto AppUser { get; set; } = null!;

    [ForeignKey("ExerciseId")]
    public virtual ExerciseDto? Exercise { get; set; }
}
