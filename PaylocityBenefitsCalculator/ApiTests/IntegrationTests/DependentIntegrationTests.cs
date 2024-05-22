using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.Dtos.Dependent;
using Api.Models;
using Xunit;

namespace ApiTests.IntegrationTests;

public class DependentIntegrationTests : IntegrationTest
{

    private List<GetDependentDto> _dependentDtos = _getEmployeeDtos.SelectMany(x => x.Dependents).ToList();



    [Fact]
    public async Task WhenAskedForAllDependents_ShouldReturnAllDependents()
    {
        var response = await HttpClient.GetAsync("/api/v1/dependents");
        await response.ShouldReturn(HttpStatusCode.OK);
    }

    [Fact]
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

