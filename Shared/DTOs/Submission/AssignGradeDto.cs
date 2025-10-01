using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Submission;

public partial class AssignGradeDto 
{
    [Key]
    public int AppUserId { get; set; }

    public int Grade { get; set; }
}
