using Api.Dtos.Dependent;
using Api.Models;
using Api.QueryRepo;

namespace Api.Services
{
    public class DependentService
    {
        private readonly DatabaseQueryRepo _databaseService;

        public DependentService(DatabaseQueryRepo databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<GetDependentDto>> GetDependantsAsync()
        {
            var dependents = await _databaseService.GetAllDependents();

            var dependentDtos = dependents.Select(d => new GetDependentDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                DateOfBirth = d.DateOfBirth,
                Relationship = d.Relationship,
                
            }).ToList();

            return dependentDtos;
        }

        public async Task<GetDependentDto> GetDependentAsync(int dependentId)
        {
            var d = await _databaseService.GetDependentById(dependentId);


            var employeeDtos = new GetDependentDto
            {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    DateOfBirth = d.DateOfBirth,
                    Relationship = d.Relationship
                
            };

            return employeeDtos;
        }

        public async Task AddDependentAsync(Dependent dependent)
        {
            var dependents = await _databaseService.GetDependentsByEmployeeId(dependent.EmployeeId);

            // Check if the dependent relationship is a spouse or domestic partner
            if (dependent.Relationship == Relationship.Spouse || dependent.Relationship == Relationship.DomesticPartner)
            {
                // Check if the employee already has a spouse or domestic partner
                var existingSpouseOrPartner = dependents.Any(d =>
                    d.Relationship == Relationship.Spouse || d.Relationship == Relationship.DomesticPartner);

                if (existingSpouseOrPartner)
                {
                    throw new InvalidOperationException("An employee can only have one spouse or domestic partner.");
                }
            }

            await _databaseService.CreateDependentAsync(dependent);
        }
    }
}

