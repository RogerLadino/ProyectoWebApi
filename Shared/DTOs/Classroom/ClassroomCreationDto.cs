using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Shared.DTOs.Classroom;
public partial class ClassroomCreationDto
{

    [Required(ErrorMessage = "El nombre del aula es requerido")]
    [StringLength(45, ErrorMessage = "El nombre no puede exceder los 45 caracteres")]
    [Display(Name = "Nombre del Aula")]
    public string Name { get; set; } = string.Empty;

}

