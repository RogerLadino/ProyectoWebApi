using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.DTOs.Submission;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("api/exercise/{exerciseId:int}/submission")]
public class SubmissionController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public SubmissionController(IServiceManager serviceManager)
        => _serviceManager = serviceManager;

    [Authorize(Roles = "Profesor")]
    [HttpGet]
    public async Task<IActionResult> GetAllSubmissions([FromRoute] int exerciseId)
    {
        var submissionDto = await _serviceManager.SubmissionService.GetAllAsync(exerciseId);
        return Ok(submissionDto);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetSubmissionById([FromRoute] int exerciseId)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
            return Unauthorized("UserId not found or invalid");

        var submissionDto = await _serviceManager.SubmissionService.GetByIdAsync(userId, exerciseId);
        return Ok(submissionDto);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetUserSubmission([FromRoute] int exerciseId, [FromRoute] int userId)
    {
        var submissionDto = await _serviceManager.SubmissionService.GetByIdAsync(userId, exerciseId);
        return Ok(submissionDto);
    }

    [Authorize(Roles = "Profesor")]
    [HttpPut]
    public async Task<IActionResult> AssignNote([FromRoute] int exerciseId, [FromBody] AssignGradeDto assignGradeDto)
    {
        await _serviceManager.SubmissionService.AssignGrade(assignGradeDto.AppUserId, exerciseId, assignGradeDto.Grade);
        return NoContent();
    }
}
