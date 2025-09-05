using Xunit;
using Moq;
using TeaLeavesProductionForecastWebAPI.Controllers;
using TeaLeavesProductionForecastWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DashboardControllerTests
{
    [Fact]
    public async Task GetAllPredictionsByUser_ReturnsOk_WhenPredictionsExist()
    {
        var mockService = new Mock<IDashboardService>();
    mockService.Setup(s => s.GetAllPredictionById(1)).ReturnsAsync(new List<TeaLeavesProductionForecastWebAPI.DTO.GetPredictionDto> { new TeaLeavesProductionForecastWebAPI.DTO.GetPredictionDto() });
        var controller = new DashboardController(mockService.Object);
        var result = await controller.GetAllPredictionsByUser(1);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetAllPredictionsByUser_ReturnsNotFound_WhenNoPredictions()
    {
        var mockService = new Mock<IDashboardService>();
    mockService.Setup(s => s.GetAllPredictionById(1)).ReturnsAsync(new List<TeaLeavesProductionForecastWebAPI.DTO.GetPredictionDto>());
        var controller = new DashboardController(mockService.Object);
        var result = await controller.GetAllPredictionsByUser(1);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetUserByIdForDashboard_ReturnsNotFound_WhenUserNull()
    {
        var mockService = new Mock<IDashboardService>();
    mockService.Setup(s => s.GetUserByIdForDashboardAsync(1)).ReturnsAsync((TeaLeavesProductionForecastWebAPI.DTO.UserResponseDto?)null);
        var controller = new DashboardController(mockService.Object);
        var result = await controller.GetUserByIdForDashboard(1);
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
