using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.DTOs.Classroom;
using System.Security.Claims;

[ApiController]
[Route("api/classroom")]
public class ClassroomController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public ClassroomController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [Authorize]
    [HttpGet("my-classrooms")]
    public async Task<ActionResult> GetMyClassrooms()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("No se pudo obtener el usuario logueado.");

        var classrooms = await _serviceManager.ClassroomService.GetByUserIdAsync(userId);
        return Ok(classrooms);
    }

    [Authorize(Roles = "Profesor")]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ClassroomCreationDto creationDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("No se pudo obtener el usuario logueado.");

        var createdClassroom = await _serviceManager.ClassroomService.CreateAndAssignProfessorAsync(creationDto, userId);
        return CreatedAtAction(nameof(GetById), new { id = createdClassroom.Id }, createdClassroom);
    }

    [Authorize(Roles = "Alumno")]
    [HttpPost("join/{code}")]
    public async Task<ActionResult> JoinByCode(string code)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("No se pudo obtener el usuario logueado.");

        await _serviceManager.ClassroomService.JoinClassroomByCodeAsync(code, userId);
        return Ok(new { Message = "Te has unido correctamente al aula." });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var classroom = await _serviceManager.ClassroomService.GetByIdAsync(id);
        return Ok(classroom);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] ClassroomUpdateDto updateDto)
    {
        await _serviceManager.ClassroomService.UpdateAsync(id, updateDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _serviceManager.ClassroomService.DeleteAsync(id);
        return NoContent();
    }
}
