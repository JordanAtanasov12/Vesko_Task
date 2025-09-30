using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NumbersAPI.Services;
using System.Net.Http.Json;
using System.Text.Json;

namespace NumbersAPI.Tests.Controllers
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetNumbers_ShouldReturnEmptyListInitially()
        {
            // Act
            var response = await _client.GetAsync("/api/numbers");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            
            Assert.Equal(0, result.GetProperty("count").GetInt32());
            Assert.Equal(0, result.GetProperty("sum").GetInt32());
            Assert.True(result.GetProperty("numbers").GetArrayLength() == 0);
        }

        [Fact]
        public async Task AddNumber_ShouldAddRandomNumber()
        {
            // Act
            var response = await _client.PostAsync("/api/numbers/add", null);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            
            Assert.Equal(1, result.GetProperty("count").GetInt32());
            Assert.Equal(1, result.GetProperty("sum").GetInt32());
            Assert.True(result.GetProperty("numbers").GetArrayLength() == 1);
            
            var number = result.GetProperty("numbers")[0];
            Assert.True(number.GetProperty("value").GetInt32() >= 1);
            Assert.True(number.GetProperty("value").GetInt32() <= 999);
        }

        [Fact]
        public async Task AddMultipleNumbers_ShouldIncrementCount()
        {
            // Act
            await _client.PostAsync("/api/numbers/add", null);
            await _client.PostAsync("/api/numbers/add", null);
            var response = await _client.PostAsync("/api/numbers/add", null);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            
            Assert.Equal(3, result.GetProperty("count").GetInt32());
            Assert.True(result.GetProperty("sum").GetInt32() > 0);
            Assert.True(result.GetProperty("numbers").GetArrayLength() == 3);
        }

        [Fact]
        public async Task ClearNumbers_ShouldRemoveAllNumbers()
        {
            // Arrange
            await _client.PostAsync("/api/numbers/add", null);
            await _client.PostAsync("/api/numbers/add", null);

            // Act
            var response = await _client.PostAsync("/api/numbers/clear", null);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            
            Assert.Equal(0, result.GetProperty("count").GetInt32());
            Assert.Equal(0, result.GetProperty("sum").GetInt32());
            Assert.True(result.GetProperty("numbers").GetArrayLength() == 0);
        }

        [Fact]
        public async Task GetSum_ShouldReturnCorrectSum()
        {
            // Arrange
            await _client.PostAsync("/api/numbers/add", null);
            await _client.PostAsync("/api/numbers/add", null);
            await _client.PostAsync("/api/numbers/add", null);

            // Act
            var response = await _client.GetAsync("/api/numbers/sum");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            
            Assert.Equal(3, result.GetProperty("count").GetInt32());
            Assert.True(result.GetProperty("sum").GetInt32() > 0);
        }

        [Fact]
        public async Task SessionPersistence_ShouldWorkAcrossRequests()
        {
            // Arrange
            await _client.PostAsync("/api/numbers/add", null);
            await _client.PostAsync("/api/numbers/add", null);

            // Act - Get numbers from session
            var response = await _client.GetAsync("/api/numbers");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonElement>(content);
            
            Assert.Equal(2, result.GetProperty("count").GetInt32());
            Assert.True(result.GetProperty("numbers").GetArrayLength() == 2);
        }

        [Fact]
        public async Task AllEndpoints_ShouldReturnValidJson()
        {
            // Act & Assert
            var getResponse = await _client.GetAsync("/api/numbers");
            getResponse.EnsureSuccessStatusCode();
            var getContent = await getResponse.Content.ReadAsStringAsync();
            Assert.True(IsValidJson(getContent));

            var addResponse = await _client.PostAsync("/api/numbers/add", null);
            addResponse.EnsureSuccessStatusCode();
            var addContent = await addResponse.Content.ReadAsStringAsync();
            Assert.True(IsValidJson(addContent));

            var sumResponse = await _client.GetAsync("/api/numbers/sum");
            sumResponse.EnsureSuccessStatusCode();
            var sumContent = await sumResponse.Content.ReadAsStringAsync();
            Assert.True(IsValidJson(sumContent));

            var clearResponse = await _client.PostAsync("/api/numbers/clear", null);
            clearResponse.EnsureSuccessStatusCode();
            var clearContent = await clearResponse.Content.ReadAsStringAsync();
            Assert.True(IsValidJson(clearContent));
        }

        private static bool IsValidJson(string jsonString)
        {
            try
            {
                JsonDocument.Parse(jsonString);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}
