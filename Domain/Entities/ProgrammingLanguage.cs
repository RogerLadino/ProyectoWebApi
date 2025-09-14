using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("ProgrammingLanguage")]
public partial class ProgrammingLanguage
{
    [Key]
    public int Id { get; set; }

    [StringLength(45)]
    public string Name { get; set; } = null!;

    [InverseProperty("ProgrammingLanguage")]
    public virtual ICollection<Code> Codes { get; set; } = new List<Code>();
}
