using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Users;

public partial class AppUserDto
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

    public int AppRoleId { get; set; }
}
