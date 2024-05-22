using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Models;
using Xunit;

namespace ApiTests.IntegrationTests;

public class DependentIntegrationTests : IntegrationTest, IAsyncLifetime
{

    private List<GetDependentDto> _dependentDtos = _getEmployeeDtos.SelectMany(x => x.Dependents).ToList();

    public async Task InitializeAsync()
    {

        foreach (var employeeDto in _getEmployeeDtos)
        {
            var employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                DateOfBirth = employeeDto.DateOfBirth,
                Salary = employeeDto.Salary,
            };
            employeeDto.Id = await _databaseQueryRepo.CreateEmployeeAsync(employee);
            if (employeeDto.Dependents.Count > 0)
            {
                foreach (var dependentDto in employeeDto.Dependents)
                {
                    var dependent = new Dependent
                    {
                        FirstName = dependentDto.FirstName,
                        LastName = dependentDto.LastName,
                        DateOfBirth = dependentDto.DateOfBirth,
                        Relationship = dependentDto.Relationship,
                        EmployeeId = employeeDto.Id
                    };

                    dependentDto.Id = await _databaseQueryRepo.CreateDependentAsync(dependent);
                }
            }
        }


    }

    public async Task DisposeAsync()
    {
        foreach (var employeeDto in _getEmployeeDtos)
        {
            await _databaseQueryRepo.DeleteDependentsAsync(employeeDto.Id);
            await _databaseQueryRepo.DeleteEmployeeAsync(employeeDto.Id);
        }

    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents");
        var dependents = _dependentDtos;
        await response.ShouldReturn(HttpStatusCode.OK, dependents);
    }

    [Fact]
    //task: make test pass
    public async Task WhenAskedForADependent_ShouldReturnCorrectDependent()
    {
        var dependent = _dependentDtos[0];
        var response = await HttpClient.GetAsync($"/api/v1/dependents/{dependent.Id}");
        await response.ShouldReturn(HttpStatusCode.OK, dependent);
    }

    [Fact]

    public async Task WhenAskedForANonexistentDependent_ShouldReturn404()
    {
        var response = await HttpClient.GetAsync($"/api/v1/dependents/{int.MinValue}");
        await response.ShouldReturn(HttpStatusCode.NotFound);
    }
}

