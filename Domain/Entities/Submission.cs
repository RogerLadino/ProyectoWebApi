using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Submission")]
public partial class Submission
{
    [Key]
    public int AppUserId { get; set; }

    [Key]
    public int ExerciseId { get; set; }

    public int Grade { get; set; }

    public byte Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SubmittedAt { get; set; }

    [ForeignKey("AppUserId")]
    [InverseProperty("Submissions")]
    public virtual AppUser AppUser { get; set; } = null!;

    [InverseProperty("Submission")]
    public virtual Code? Code { get; set; }

    [ForeignKey("ExerciseId")]
    [InverseProperty("Submissions")]
    public virtual Exercise Exercise { get; set; } = null!;
}
