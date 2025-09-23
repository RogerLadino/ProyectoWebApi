using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.DTOs.Classroom;

namespace Presentation.Controllers;

[ApiController]
[Route("api/classroom")] // ruta clara
public class ClassroomController : ControllerBase
{
    private readonly IClassroomService _classroomService;

    public ClassroomController(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    // GET: api/classrooms
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var classrooms = await _classroomService.GetAllAsync();
        return Ok(classrooms);
    }

    // GET: api/classrooms/5
    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var classroom = await _classroomService.GetByIdAsync(id);
        return Ok(classroom);
    }

    // GET: api/classrooms/code/MATH-101
    [HttpGet("code/{code}")]
    public async Task<ActionResult> GetByCode(string code)
    {
        var classroom = await _classroomService.GetByCodeAsync(code);
        return Ok(classroom);
    }

    // POST: api/classrooms
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ClassroomCreationDto creationDto)
    {
        var createdClassroom = await _classroomService.CreateAsync(creationDto);
        return CreatedAtAction(nameof(GetById), new { id = createdClassroom.Id }, createdClassroom);
    }

    // PUT: api/classrooms/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] ClassroomDto updateDto)
    {
        await _classroomService.UpdateAsync(id, updateDto);
        return NoContent(); // 204 No Content
    }

    // DELETE: api/classrooms/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _classroomService.DeleteAsync(id);
        return NoContent(); // 204 No Content
    }
}