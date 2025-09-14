using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.DTOs.Exercise;

[ApiController]
[Route("api/exercises")]
public class ExercisesController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public ExercisesController(IServiceManager serviceManager)
        => _serviceManager = serviceManager;

    [HttpGet]
    public async Task<IActionResult> GetAllExercises()
    {
        var exercisesDto = await _serviceManager.ExerciseService.GetAllAsync();
        return Ok(exercisesDto);
    }

    [HttpGet("{exerciseId:int}")]
    public async Task<IActionResult> GetExerciseById(int exerciseId)
    {
        var exerciseDto = await _serviceManager.ExerciseService.GetByIdAsync(exerciseId);
        return Ok(exerciseDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExercise([FromBody] ExerciseCreationDto exerciseForCreationDto)
    {
        var createdExercise = await _serviceManager.ExerciseService.CreateAsync(exerciseForCreationDto);
        return CreatedAtAction(nameof(GetExerciseById), new { exerciseId = createdExercise.Id }, createdExercise);
    }

    [HttpPut("{exerciseId:int}")]
    public async Task<IActionResult> UpdateExercise(int exerciseId, [FromBody] ExerciseDto exerciseForUpdateDto)
    {
        await _serviceManager.ExerciseService.UpdateAsync(exerciseId, exerciseForUpdateDto);
        return NoContent();
    }

    [HttpDelete("{exerciseId:int}")]
    public async Task<IActionResult> DeleteExercise(int exerciseId)
    {
        await _serviceManager.ExerciseService.DeleteAsync(exerciseId);
        return NoContent();
    }
}
