using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Paycheck;
using Api.Models;
using Xunit;

namespace ApiTests.IntegrationTests;

public class EmployeeIntegrationTests : IntegrationTest
{



    [Fact]
    public async Task WhenAskedForAllEmployees_ShouldReturnAllEmployees()
    {
        var response = await HttpClient.GetAsync("/api/v1/employees");
        await response.ShouldReturn(HttpStatusCode.OK);
    }

    [Fact]
    public async Task WhenAskedForAnEmployee_ShouldReturnCorrectEmployee()
    {
        var employee = _getEmployeeDtos[0];
        var response = await HttpClient.GetAsync($"/api/v1/employees/{employee.Id}");
        await response.ShouldReturn(HttpStatusCode.OK, employee);
    }


    [Fact]
    public async Task WhenAskedForAnEmployeePaycheckAllDeductionsOneDependent_ShouldReturnCorrectPaycheck()
    {
        var employee = _getEmployeeDtos.Find(e => e.FirstName == "Michael");
        var response = await HttpClient.GetAsync($"/api/v1/employees/{employee.Id}/paychecks/1");
        var paycheckDto = new GetPaycheckDto
        {
            GrossTotal = 5508.12m,
            NetTotal = 4597.95m,
        };
        await response.ShouldReturn(HttpStatusCode.OK, paycheckDto);
    }

    [Fact]
    public async Task WhenAskedForAnEmployeePaycheckOutofRange_ShouldReturError()
    {
        var employee = _getEmployeeDtos.Find(e => e.FirstName == "Michael");
        var response = await HttpClient.GetAsync($"/api/v1/employees/{employee.Id}/paychecks/30");
        await response.ShouldReturn(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task WhenAskedForAnEmployeePaycheckOnlyBaseDeduction_ShouldReturnOkay()
    {
        var employee = _getEmployeeDtos.Find(e => e.FirstName == "LeBron");
        var response = await HttpClient.GetAsync($"/api/v1/employees/{employee.Id}/paychecks/1");
        var paycheckDto = new GetPaycheckDto
        {
            GrossTotal = 2900.81m,
            NetTotal = 2400.81m,
        };
        await response.ShouldReturn(HttpStatusCode.OK, paycheckDto);
    }

    [Fact]
    public async Task WhenAskedForAnEmployeePaycheckBaseAndRegularDependentDeduction_ShouldReturOkay()
    {
        var employee = _getEmployeeDtos.Find(e => e.FirstName == "Ja");
        var response = await HttpClient.GetAsync($"/api/v1/employees/{employee.Id}/paychecks/1");
        var paycheckDto = new GetPaycheckDto
        {
            GrossTotal = 3552.51m,
            NetTotal = 2081.45m,
        };
        await response.ShouldReturn(HttpStatusCode.OK, paycheckDto);
    }


    [Fact]
    public async Task WhenAskedForANonexistentEmployee_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/employees/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }
}

