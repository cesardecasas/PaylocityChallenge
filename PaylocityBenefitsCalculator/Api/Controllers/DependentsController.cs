using Api.Dtos.Dependent;
using Api.Models;
using Api.QueryRepo;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{

    private readonly DatabaseQueryRepo _databaseService;
    private readonly DependentService _dependentService;

    public DependentsController(DatabaseQueryRepo databaseService, DependentService dependentService)
    {
        _databaseService = databaseService;
        _dependentService = dependentService;
    }
    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await _dependentService.GetDependentAsync(id);

        if (dependent == null || dependent.Id == 0)
        {
            return NotFound("Dependent with id " + id + " was not found");
        }

        var result = new ApiResponse<GetDependentDto>
        {
            Data = dependent,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _dependentService.GetDependantsAsync();


        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Add a dependent to employee")]
    [HttpPost]
    public async Task<IActionResult> AddDependant([FromBody] AddDependentRequest dependentRequest)
    {
        try
        {
            var dependent = new Dependent
            {
                FirstName = dependentRequest.FirstName,
                LastName = dependentRequest.LastName,
                EmployeeId = dependentRequest.EmployeeId,
                Relationship = dependentRequest.Relationship,
                DateOfBirth = dependentRequest.DateOfBirth,
            };
            await _dependentService.AddDependentAsync(dependent);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
