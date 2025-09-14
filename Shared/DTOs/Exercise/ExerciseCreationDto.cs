using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Exercise;

public partial class ExerciseCreationDto
{
    public int ClassroomId { get; set; }

    [StringLength(45)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Description { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? DueDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }
}
