using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.DTOs.Submission;

[ApiController]
[Route("api/classroom/{classroomId:int}/exercise/{exerciseId:int}/submission")]
public class SubmissionController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public SubmissionController(IServiceManager serviceManager)
        => _serviceManager = serviceManager;

    [HttpGet]
    public async Task<IActionResult> GetAllSubmissions([FromRoute] int exerciseId)
    {
        var submissionDto = await _serviceManager.SubmissionService.GetAllAsync(exerciseId);
        return Ok(submissionDto);
    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetSubmissionById([FromRoute] int userId, [FromRoute] int exerciseId)
    {
        var submissionDto = await _serviceManager.SubmissionService.GetByIdAsync(userId, exerciseId);
        return Ok(submissionDto);
    }

    [HttpPut]
    public async Task<IActionResult> AssignNote([FromRoute] int exerciseId, [FromBody] AssignGradeDto assignGradeDto)
    {
        await _serviceManager.SubmissionService.AssignGrade(assignGradeDto.AppUserId, exerciseId, assignGradeDto.Grade);
        return NoContent();
    }
}
