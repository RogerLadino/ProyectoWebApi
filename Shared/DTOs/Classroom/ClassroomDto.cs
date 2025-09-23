using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DTOs.Classroom;
public partial class ClassroomDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

}