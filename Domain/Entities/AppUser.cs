using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    // Atributo personalizado para evitar repetir [StringLength(45)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NameLengthAttribute : StringLengthAttribute
    {
        public NameLengthAttribute() : base(45) { }
    }

    [Table("AppUser")]
    public partial class AppUser
    {
        [Key]
        public int Id { get; set; }

        [EmailAddress]
        [NameLength]
        public string Email { get; set; } = null!;

        [NameLength]
        public string FirstName { get; set; } = null!;

        [NameLength]
        public string? MiddleName { get; set; }

        [NameLength]
        public string LastName { get; set; } = null!;

        [NameLength]
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
}
