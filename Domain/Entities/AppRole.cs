using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("AppRole")]
public partial class AppRole
{
    [Key]
    public int Id { get; set; }

    [StringLength(45)]
    public string Description { get; set; } = null!;

    [InverseProperty("AppRole")]
    public virtual ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>();
}
