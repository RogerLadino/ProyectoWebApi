using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("AppUser")]
public partial class AppUser
{
    [Key]
    public int Id { get; set; }

    [EmailAddress]
    [StringLength(45)]
    public string Email { get; set; } = null!;

    [StringLength(45)]
    public string FirstName { get; set; } = null!;

    [StringLength(45)]
    public string? MiddleName { get; set; }

    [StringLength(45)]
    public string LastName { get; set; } = null!;

    [StringLength(45)]
    public string? SecondLastName { get; set; }

    [StringLength(128)]
    public string Password { get; set; } = null!;

    public int AppRoleId { get; set; }

    [ForeignKey("AppRoleId")]
    [InverseProperty("AppUsers")]
    public virtual AppRole AppRole { get; set; } = null!;
    public string? TokenResetPassword { get; set; }
    public DateTime? FechaExpiracionToken { get; set; }

    [InverseProperty("AppUser")]
    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    [ForeignKey("AppUserId")]
    [InverseProperty("AppUsers")]
    public virtual ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();
}