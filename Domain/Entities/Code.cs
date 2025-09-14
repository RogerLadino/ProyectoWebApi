using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Code")]
public partial class Code
{
    [Key]
    public int AppUserId { get; set; }

    [Key]
    public int ExerciseId { get; set; }

    [Column(TypeName = "text")]
    public string SourceCode { get; set; } = null!;

    public int Attempts { get; set; }

    public int ProgrammingLanguageId { get; set; }

    [ForeignKey("ProgrammingLanguageId")]
    [InverseProperty("Codes")]
    public virtual ProgrammingLanguage ProgrammingLanguage { get; set; } = null!;

    [ForeignKey("AppUserId, ExerciseId")]
    [InverseProperty("Code")]
    public virtual Submission Submission { get; set; } = null!;
}
