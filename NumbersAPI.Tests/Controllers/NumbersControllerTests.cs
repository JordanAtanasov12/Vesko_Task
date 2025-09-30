using Microsoft.AspNetCore.Mvc;
using Moq;
using NumbersAPI.Controllers;
using NumbersAPI.Models;
using NumbersAPI.Services;
using NumbersAPI.Tests.TestHelpers;

namespace NumbersAPI.Tests.Controllers
{
    public class NumbersControllerTests
    {
        private readonly Mock<INumberService> _mockNumberService;
        private readonly NumbersController _controller;

        public NumbersControllerTests()
        {
            _mockNumberService = new Mock<INumberService>();
            _controller = new NumbersController(_mockNumberService.Object);
        }

        [Fact]
        public async Task GetNumbers_WhenServiceReturnsData_ShouldReturnOkResult()
        {
            // Arrange
            var expectedResponse = TestDataBuilder.CreateNumberResponse(TestDataBuilder.CreateNumberList(3));
            _mockNumberService.Setup(s => s.GetNumbersAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetNumbers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<NumberResponse>(okResult.Value);
            Assert.Equal(expectedResponse.Count, response.Count);
            Assert.Equal(expectedResponse.Sum, response.Sum);
            Assert.Equal(expectedResponse.Numbers.Count, response.Numbers.Count);
        }

        [Fact]
        public async Task GetNumbers_WhenServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            _mockNumberService.Setup(s => s.GetNumbersAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetNumbers();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task AddNumber_WhenServiceReturnsData_ShouldReturnOkResult()
        {
            // Arrange
            var expectedResponse = TestDataBuilder.CreateNumberResponse(TestDataBuilder.CreateNumberList(1));
            _mockNumberService.Setup(s => s.AddRandomNumberAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.AddNumber();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<NumberResponse>(okResult.Value);
            Assert.Equal(expectedResponse.Count, response.Count);
        }

        [Fact]
        public async Task AddNumber_WhenServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            _mockNumberService.Setup(s => s.AddRandomNumberAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.AddNumber();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task ClearNumbers_WhenServiceReturnsData_ShouldReturnOkResult()
        {
            // Arrange
            var expectedResponse = TestDataBuilder.CreateNumberResponse(new List<NumberItem>());
            _mockNumberService.Setup(s => s.ClearNumbersAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ClearNumbers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<NumberResponse>(okResult.Value);
            Assert.Equal(0, response.Count);
            Assert.Empty(response.Numbers);
        }

        [Fact]
        public async Task ClearNumbers_WhenServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            _mockNumberService.Setup(s => s.ClearNumbersAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.ClearNumbers();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task GetSum_WhenServiceReturnsData_ShouldReturnOkResult()
        {
            // Arrange
            var numbers = TestDataBuilder.CreateNumberList(3);
            var expectedResponse = TestDataBuilder.CreateNumberResponse(numbers);
            _mockNumberService.Setup(s => s.GetSumAsync()).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetSum();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<NumberResponse>(okResult.Value);
            Assert.Equal(expectedResponse.Sum, response.Sum);
            Assert.Equal(expectedResponse.Count, response.Count);
        }

        [Fact]
        public async Task GetSum_WhenServiceThrowsException_ShouldReturnInternalServerError()
        {
            // Arrange
            _mockNumberService.Setup(s => s.GetSumAsync()).ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetSum();

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }

        [Fact]
        public async Task GetNumbers_ShouldCallServiceGetNumbersAsync()
        {
            // Arrange
            var expectedResponse = TestDataBuilder.CreateNumberResponse(TestDataBuilder.CreateNumberList(2));
            _mockNumberService.Setup(s => s.GetNumbersAsync()).ReturnsAsync(expectedResponse);

            // Act
            await _controller.GetNumbers();

            // Assert
            _mockNumberService.Verify(s => s.GetNumbersAsync(), Times.Once);
        }

        [Fact]
        public async Task AddNumber_ShouldCallServiceAddRandomNumberAsync()
        {
            // Arrange
            var expectedResponse = TestDataBuilder.CreateNumberResponse(TestDataBuilder.CreateNumberList(1));
            _mockNumberService.Setup(s => s.AddRandomNumberAsync()).ReturnsAsync(expectedResponse);

            // Act
            await _controller.AddNumber();

            // Assert
            _mockNumberService.Verify(s => s.AddRandomNumberAsync(), Times.Once);
        }

        [Fact]
        public async Task ClearNumbers_ShouldCallServiceClearNumbersAsync()
        {
            // Arrange
            var expectedResponse = TestDataBuilder.CreateNumberResponse(new List<NumberItem>());
            _mockNumberService.Setup(s => s.ClearNumbersAsync()).ReturnsAsync(expectedResponse);

            // Act
            await _controller.ClearNumbers();

            // Assert
            _mockNumberService.Verify(s => s.ClearNumbersAsync(), Times.Once);
        }

        [Fact]
        public async Task GetSum_ShouldCallServiceGetSumAsync()
        {
            // Arrange
            var expectedResponse = TestDataBuilder.CreateNumberResponse(TestDataBuilder.CreateNumberList(3));
            _mockNumberService.Setup(s => s.GetSumAsync()).ReturnsAsync(expectedResponse);

            // Act
            await _controller.GetSum();

            // Assert
            _mockNumberService.Verify(s => s.GetSumAsync(), Times.Once);
        }
    }
}
