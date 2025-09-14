using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Exercise")]
public partial class Exercise
{
    [Key]
    public int Id { get; set; }

    public int ClassroomId { get; set; }

    [StringLength(45)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Description { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? DueDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ClassroomId")]
    [InverseProperty("Exercises")]
    public virtual Classroom Classroom { get; set; } = null!;

    [InverseProperty("Exercise")]
    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    [InverseProperty("Exercise")]
    public virtual ICollection<TestCase> TestCases { get; set; } = new List<TestCase>();
}
