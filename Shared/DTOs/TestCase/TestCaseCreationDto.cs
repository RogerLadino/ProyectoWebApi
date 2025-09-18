using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("TestCase")]
public partial class TestCaseCreationDto
{
    [StringLength(100)]
    public string FunctionName { get; set; } = null!;

    [Column(TypeName = "text")]
    public string InputData { get; set; } = null!;

    [Column(TypeName = "text")]
    public string ExpectedOutput { get; set; } = null!;
}
