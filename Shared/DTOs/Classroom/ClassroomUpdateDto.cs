using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Classroom;

public class ClassroomUpdateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(45, ErrorMessage = "El nombre no puede exceder los 45 caracteres")]
    [Display(Name = "Nombre del Aula")]
    public string Name { get; set; } = null!;
}
