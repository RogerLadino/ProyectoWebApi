using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Classroom")]
public partial class Classroom
{
    [Key]
    public int Id { get; set; }

    [StringLength(45)]
    public string Name { get; set; } = null!;

    [StringLength(20)]
    public string Code { get; set; } = null!;

    [InverseProperty("Classroom")]
    public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

    [ForeignKey("ClassroomId")]
    [InverseProperty("Classrooms")]
    public virtual ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>();
}
