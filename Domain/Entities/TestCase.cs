using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("TestCase")]
public partial class TestCase
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string FunctionName { get; set; } = null!;

    [Column(TypeName = "text")]
    public string InputData { get; set; } = null!;

    [Column(TypeName = "text")]
    public string ExpectedOutput { get; set; } = null!;

    public int ExerciseId { get; set; }

    [ForeignKey("ExerciseId")]
    [InverseProperty("TestCases")]
    public virtual Exercise Exercise { get; set; } = null!;
}
