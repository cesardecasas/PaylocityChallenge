using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using Api.QueryRepo;

namespace Api.Services
{
    public class EmployeeService
    {
        private readonly DatabaseQueryRepo _databaseService;

        public EmployeeService(DatabaseQueryRepo databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<GetEmployeeDto>> GetEmployeesWithDependentsAsync()
        {
            var employees = await _databaseService.GetAllEmployees();
            var dependents = await _databaseService.GetAllDependents();

            foreach (var employee in employees)
            {
                employee.Dependents = dependents.FindAll(d => d.EmployeeId == employee.Id);
            }

            var employeeDtos = employees.Select(e => new GetEmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Salary = e.Salary,
                DateOfBirth = e.DateOfBirth,
                Dependents = e.Dependents.Select(d => new GetDependentDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    DateOfBirth = d.DateOfBirth,
                    Relationship = d.Relationship
                }).ToList()
            }).ToList();

            return employeeDtos;
        }

        public async Task<GetEmployeeDto> GetEmployeeWithDependentsAsync(int employeeId)
        {
            var employee = await _databaseService.GetEmployeeById(employeeId);
            var dependents = await _databaseService.GetDependentsByEmployeeId(employeeId);
            employee.Dependents = dependents;
            

            var employeeDtos = new GetEmployeeDto
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Salary = employee.Salary,
                DateOfBirth = employee.DateOfBirth,
                Dependents = employee.Dependents.Select(d => new GetDependentDto
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    DateOfBirth = d.DateOfBirth,
                    Relationship = d.Relationship
                }).ToList()
            };

            return employeeDtos;
        }

        public async Task<int> AddEmployee(Employee employee)
        {
            return await _databaseService.CreateEmployeeAsync(employee);
        }
    }
}
