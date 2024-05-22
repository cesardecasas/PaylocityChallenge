using Api.Dtos.Employee;
using Api.Dtos.Paycheck;
using Api.Models;
using Api.QueryRepo;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly DatabaseQueryRepo _databaseService;
    private readonly EmployeeService _employeeService;
    private readonly PaycheckService _paycheckService;

    public EmployeesController(DatabaseQueryRepo databaseService, EmployeeService employeeService, PaycheckService paycheckService)
    {
        _databaseService = databaseService;
        _employeeService = employeeService;
        _paycheckService = paycheckService;
    }


    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = await _employeeService.GetEmployeeWithDependentsAsync(id);

        if (employee == null || employee.Id == 0)
        {
            return NotFound("Employee with id " + id + " was not found");
        }
        var result = new ApiResponse<GetEmployeeDto>
        {
            Data = employee,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        try
        {
            var employees = await _employeeService.GetEmployeesWithDependentsAsync();

            var result = new ApiResponse<List<GetEmployeeDto>>
            {
                Data = employees,
                Success = true
            };

            return result;
        }
        catch (Exception ex) 
        {
            return BadRequest(new { message = ex.Message });
        }

        
    }

    [SwaggerOperation(Summary = "Add an employee")]
    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] CreateEmployeeRequest createEmployeeRequest)
    {
        try
        {
            var dependent = new Employee
            {
                FirstName = createEmployeeRequest.FirstName,
                LastName = createEmployeeRequest.LastName,
                DateOfBirth = createEmployeeRequest.DateOfBirth,
                Salary = createEmployeeRequest.Salary
            };
            var id =  await _employeeService.AddEmployee(dependent);
            var result = new ApiResponse<int>
            {
                Data = id,
                Success = true
            };
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [SwaggerOperation(Summary = "Get year round employee paychecks")]
    [HttpGet("{id}/paychecks")]
    public async Task<ActionResult<ApiResponse<List<GetPaycheckDto>>>> GetPaychecks(int id)
    {
        var paychecks = await _paycheckService.GetPaychecksByEmployee(id);

        if(paychecks == null || paychecks.Count == 0)
        {
            return NotFound("Paychecks couldn't be generated for employee id: " + id);
        }

        
        var result = new ApiResponse<List<GetPaycheckDto>>
        {
            Data = paychecks,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get paycheck by number")]
    [HttpGet("{id}/paychecks/{number}")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> GetPaychecks(int id, int number)
    {
        if(number <= 0 || number > 26)
        {
            throw new ArgumentOutOfRangeException(nameof(number), "Paycheck number must be between 1 and 26.");
        }
        var paycheck = await _paycheckService.GetPaycheckByNumber(id, number);

        if (paycheck == null)
        {
            return NotFound("Paycheck couldn't be generated for employee id: " + id);
        }


        var result = new ApiResponse<GetPaycheckDto>
        {
            Data = paycheck,
            Success = true
        };

        return result;
    }
}
