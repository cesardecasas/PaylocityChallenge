using Api.Dtos.Employee;
using Api.QueryRepo;
using System;
using Api.Dtos.Dependent;
using Api.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiTests;

public class IntegrationTest : IDisposable
{
    private HttpClient? _httpClient;
    protected DatabaseQueryRepo _databaseQueryRepo;
    protected static List<GetEmployeeDto> _getEmployeeDtos = new List<GetEmployeeDto>
        {
            new()
            {

                FirstName = "LeBron",
                LastName = "James",
                Salary = 75420.99m,
                DateOfBirth = new DateTime(1984, 12, 30)
            },
            new()
            {

                FirstName = "Ja",
                LastName = "Morant",
                Salary = 92365.22m,
                DateOfBirth = new DateTime(1999, 8, 10),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {

                        FirstName = "Spouse",
                        LastName = "Morant",
                        Relationship = Relationship.Spouse,
                        DateOfBirth = new DateTime(1998, 3, 3)
                    },
                    new()
                    {
                        FirstName = "Child1",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2020, 6, 23)
                    },
                    new()
                    {

                        FirstName = "Child2",
                        LastName = "Morant",
                        Relationship = Relationship.Child,
                        DateOfBirth = new DateTime(2021, 5, 18)
                    }
                }
            },
            new()
            {

                FirstName = "Michael",
                LastName = "Jordan",
                Salary = 143211.12m,
                DateOfBirth = new DateTime(1963, 2, 17),
                Dependents = new List<GetDependentDto>
                {
                    new()
                    {

                        FirstName = "DP",
                        LastName = "Jordan",
                        Relationship = Relationship.DomesticPartner,
                        DateOfBirth = new DateTime(1974, 1, 2)
                    }
                }
            }
        };

    public IntegrationTest()
    {
        var connectionString = "Host=localhost;Port=5432;Database=paylocity_benefits.test;Username=postgres;Password=postgres;";
        _databaseQueryRepo = new DatabaseQueryRepo(connectionString);
        SeedDatabase().GetAwaiter().GetResult();
    }

    protected HttpClient HttpClient
    {
        get
        {
            if (_httpClient == default)
            {
                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:7124")
                };
                _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
            }

            return _httpClient;
        }
    }

    private async Task SeedDatabase()
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

    public void Dispose()
    {
        foreach (var employeeDto in _getEmployeeDtos)
        {
             _databaseQueryRepo.DeleteDependentsAsync(employeeDto.Id);
             _databaseQueryRepo.DeleteEmployeeAsync(employeeDto.Id);
        }
        HttpClient.Dispose();
    }
}

