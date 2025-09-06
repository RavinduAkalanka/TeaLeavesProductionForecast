using Xunit;
using Moq;
using TeaLeavesProductionForecastWebAPI.Controllers;
using TeaLeavesProductionForecastWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PredictionControllerTests
{
    [Fact]
    public async Task GetPrediction_ReturnsOk()
    {
        var mockService = new Mock<IPredictionService>();
    mockService.Setup(s => s.PredictionAsync(It.IsAny<TeaLeavesProductionForecastWebAPI.DTO.PredictionDto>())).ReturnsAsync(123.45m);
        var controller = new PredictionController(mockService.Object);
        var dto = new TeaLeavesProductionForecastWebAPI.DTO.PredictionDto();
        var result = await controller.GetPrediction(dto);
        Assert.IsType<OkObjectResult>(result);
    }
}
