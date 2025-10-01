using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Code;

public partial class CodeUpdateDto
{
    [Key]
    public int AppUserId { get; set; }

    [Key]
    public int ExerciseId { get; set; }

    [Column(TypeName = "text")]
    public string SourceCode { get; set; } = null!;
}
