using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Users
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NameLengthAttribute : StringLengthAttribute
    {
        public NameLengthAttribute() : base(45) { }
    }

    public partial class AppUserDto
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

        public int AppRoleId { get; set; }
    }
}
