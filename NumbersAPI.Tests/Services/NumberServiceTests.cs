using Microsoft.AspNetCore.Http;
using Moq;
using NumbersAPI.Models;
using NumbersAPI.Services;
using NumbersAPI.Tests.TestHelpers;
using System.Text.Json;

namespace NumbersAPI.Tests.Services
{
    public class NumberServiceTests
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly NumberService _numberService;

        public NumberServiceTests()
        {
            _mockHttpContextAccessor = MockHttpContextAccessor.CreateMock();
            _numberService = new NumberService(_mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task AddRandomNumberAsync_WhenCalled_ShouldAddNumberToSession()
        {
            // Arrange
            var existingNumbers = TestDataBuilder.CreateNumberList(2);
            var existingNumbersJson = JsonSerializer.Serialize(existingNumbers);
            _mockHttpContextAccessor.Object.HttpContext!.Session.SetString("Numbers", existingNumbersJson);

            // Act
            var result = await _numberService.AddRandomNumberAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(3, result.Numbers.Count);
            Assert.True(result.Numbers.Any(n => n.Id == 3));
            Assert.True(result.Numbers.Last().Value >= 1 && result.Numbers.Last().Value <= 999);
        }

        [Fact]
        public async Task AddRandomNumberAsync_WhenNoExistingNumbers_ShouldAddFirstNumber()
        {
            // Arrange - No existing numbers in session

            // Act
            var result = await _numberService.AddRandomNumberAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
            Assert.Single(result.Numbers);
            Assert.Equal(1, result.Numbers.First().Id);
            Assert.True(result.Numbers.First().Value >= 1 && result.Numbers.First().Value <= 999);
        }

        [Fact]
        public async Task ClearNumbersAsync_WhenCalled_ShouldClearAllNumbers()
        {
            // Arrange
            var existingNumbers = TestDataBuilder.CreateNumberList(3);
            var existingNumbersJson = JsonSerializer.Serialize(existingNumbers);
            _mockHttpContextAccessor.Object.HttpContext!.Session.SetString("Numbers", existingNumbersJson);

            // Act
            var result = await _numberService.ClearNumbersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
            Assert.Empty(result.Numbers);
            Assert.Equal(0, result.Sum);
        }

        [Fact]
        public async Task GetNumbersAsync_WhenNumbersExist_ShouldReturnAllNumbers()
        {
            // Arrange
            var expectedNumbers = TestDataBuilder.CreateNumberList(3);
            var numbersJson = JsonSerializer.Serialize(expectedNumbers);
            _mockHttpContextAccessor.Object.HttpContext!.Session.SetString("Numbers", numbersJson);

            // Act
            var result = await _numberService.GetNumbersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(3, result.Numbers.Count);
            Assert.Equal(expectedNumbers.Sum(n => n.Value), result.Sum);
        }

        [Fact]
        public async Task GetNumbersAsync_WhenNoNumbers_ShouldReturnEmptyList()
        {
            // Arrange - No numbers in session

            // Act
            var result = await _numberService.GetNumbersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
            Assert.Empty(result.Numbers);
            Assert.Equal(0, result.Sum);
        }

        [Fact]
        public async Task GetSumAsync_WhenNumbersExist_ShouldReturnCorrectSum()
        {
            // Arrange
            var numbers = TestDataBuilder.CreateNumberList(3);
            var numbersJson = JsonSerializer.Serialize(numbers);
            _mockHttpContextAccessor.Object.HttpContext!.Session.SetString("Numbers", numbersJson);
            var expectedSum = numbers.Sum(n => n.Value);

            // Act
            var result = await _numberService.GetSumAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSum, result.Sum);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetSumAsync_WhenNoNumbers_ShouldReturnZero()
        {
            // Arrange - No numbers in session

            // Act
            var result = await _numberService.GetSumAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Sum);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public async Task AddRandomNumberAsync_WhenCalledMultipleTimes_ShouldIncrementIds()
        {
            // Arrange - No existing numbers

            // Act
            var result1 = await _numberService.AddRandomNumberAsync();
            var result2 = await _numberService.AddRandomNumberAsync();
            var result3 = await _numberService.AddRandomNumberAsync();

            // Assert
            Assert.Equal(1, result1.Numbers.First().Id);
            Assert.Equal(2, result2.Numbers.Count);
            Assert.Equal(2, result2.Numbers.Last().Id);
            Assert.Equal(3, result3.Numbers.Count);
            Assert.Equal(3, result3.Numbers.Last().Id);
        }

        [Fact]
        public async Task AddRandomNumberAsync_WhenCalled_ShouldGenerateRandomValues()
        {
            // Arrange - No existing numbers

            // Act
            var result1 = await _numberService.AddRandomNumberAsync();
            var result2 = await _numberService.AddRandomNumberAsync();
            var result3 = await _numberService.AddRandomNumberAsync();

            // Assert
            var values = new[] { result1.Numbers.First().Value, result2.Numbers.Last().Value, result3.Numbers.Last().Value };
            
            // Check that values are within expected range
            Assert.All(values, value => Assert.True(value >= 1 && value <= 999));
            
            // Check that we got some different values (very high probability with 3 random numbers)
            Assert.True(values.Distinct().Count() > 1);
        }

        [Fact]
        public async Task GetNumbersAsync_WhenSessionIsNull_ShouldReturnEmptyList()
        {
            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(a => a.HttpContext).Returns((HttpContext)null);
            var service = new NumberService(mockHttpContextAccessor.Object);

            // Act
            var result = await service.GetNumbersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
            Assert.Empty(result.Numbers);
            Assert.Equal(0, result.Sum);
        }

        [Fact]
        public async Task AddRandomNumberAsync_WhenSessionIsNull_ShouldNotThrow()
        {
            // Arrange
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(a => a.HttpContext).Returns((HttpContext)null);
            var service = new NumberService(mockHttpContextAccessor.Object);

            // Act & Assert
            var result = await service.AddRandomNumberAsync();
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
        }
    }
}
