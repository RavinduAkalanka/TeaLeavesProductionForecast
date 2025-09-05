using Xunit;
using Moq;
using TeaLeavesProductionForecastWebAPI.Controllers;
using TeaLeavesProductionForecastWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserControllerTests
{
    [Fact]
    public async Task GetUserById_ReturnsOk()
    {
        var mockService = new Mock<IUserService>();
    mockService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync(new TeaLeavesProductionForecastWebAPI.DTO.UserResponseDto());
        var controller = new UserController(mockService.Object);
        var result = await controller.GetUserById(1);
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task GetUserById_ReturnsNotFound_WhenUserNull()
    {
        var mockService = new Mock<IUserService>();
    mockService.Setup(s => s.GetUserByIdAsync(1)).ReturnsAsync((TeaLeavesProductionForecastWebAPI.DTO.UserResponseDto?)null);
        var controller = new UserController(mockService.Object);
        var result = await controller.GetUserById(1);
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
