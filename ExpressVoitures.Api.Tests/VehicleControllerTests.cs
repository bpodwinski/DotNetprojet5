using ExpressVoituresApi.Controllers;
using ExpressVoituresApi.Models.Dtos;
using ExpressVoituresApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExpressVoituresApi.Tests
{
    public class VehicleControllerTests
    {
        private readonly Mock<IVehicleService> _mockVehicleService;
        private readonly Mock<ILogger<VehicleController>> _mockLogger;
        private readonly VehicleController _vehicleController;

        public VehicleControllerTests()
        {
            _mockVehicleService = new Mock<IVehicleService>();
            _mockLogger = new Mock<ILogger<VehicleController>>();
            _vehicleController = new VehicleController(_mockLogger.Object, _mockVehicleService.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfVehicles()
        {
            // Arrange
            var vehicleList = new List<VehicleDto>
            {
                new VehicleDto { id = 1, brand = "Toyota", model = "Corolla" },
                new VehicleDto { id = 2, brand = "Honda", model = "Civic" },
                new VehicleDto { id = 3, brand = "BMW", model = "E46" }
            };

            _mockVehicleService.Setup(service => service.GetAllVehicles(1, 25, null, null))
                               .ReturnsAsync(vehicleList);

            // Act
            var result = await _vehicleController.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<VehicleDto>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
        }
    }
}