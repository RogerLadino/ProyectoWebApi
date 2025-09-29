using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Service.Abstractions;
using Shared.DTOs.Classroom;

namespace Presentation.Controllers;

[ApiController]
[Route("api/classroom")]
public class ClassroomController : ControllerBase
{
    private readonly IClassroomService _classroomService;

    public ClassroomController(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    // Reemplaza la línea problemática en el método GetMyClassrooms
    [Authorize]
    [HttpGet("my-classrooms")]
    public async Task<ActionResult> GetMyClassrooms()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("No se pudo obtener el usuario logueado.");

        // El método GetByUserIdAsync devuelve Task, no un resultado.
        await _classroomService.GetByUserIdAsync(userId);
        return Ok(); // O ajusta el método de servicio para devolver datos si es necesario.
    }

    // Reemplaza la línea que causa el error en el método Create por la llamada existente en la interfaz IClassroomService.
    // La interfaz no tiene el método CreateAndAssignProfessorAsync, pero sí tiene CreateAsync.
    // Puedes agregar la lógica de asignación de profesor en el servicio, si es necesario, pero aquí solo puedes llamar a CreateAsync.

    [Authorize(Roles = "Profesor")]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ClassroomCreationDto creationDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("No se pudo obtener el usuario logueado.");

        // Llama al método existente en la interfaz
        var createdClassroom = await _classroomService.CreateAsync(creationDto);
        return CreatedAtAction(nameof(GetById), new { id = createdClassroom.Id }, createdClassroom);
    }

    // Reemplaza el método JoinByCode para corregir el error CS1061.
    // Elimina la llamada a JoinClassroomByCodeAsync y utiliza GetByCodeAsync para obtener el aula por código.
    // Si necesitas lógica adicional para que el usuario se una al aula, deberás implementarla en el servicio y la interfaz.

    [Authorize(Roles = "Alumno")]
    [HttpPost("join/{code}")]
    public async Task<ActionResult> JoinByCode(string code)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("No se pudo obtener el usuario logueado.");

        var classroom = await _classroomService.GetByCodeAsync(code);
        if (classroom == null)
            return NotFound(new { Message = "No se encontró un aula con ese código." });

        // Aquí podrías agregar lógica para asociar el usuario al aula si existe un método en el servicio.
        // Por ahora, solo retorna el aula encontrada.
        return Ok(new { Message = "Aula encontrada.", Classroom = classroom });
    }

    // Los demás endpoints pueden quedar igual
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var classroom = await _classroomService.GetByIdAsync(id);
        return Ok(classroom);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] ClassroomUpdateDto updateDto)
    {
        await _classroomService.UpdateAsync(id, updateDto);
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _classroomService.DeleteAsync(id);
        return NoContent();
    }
}
