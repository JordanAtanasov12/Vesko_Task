using NumbersAPI.Models;

namespace NumbersAPI.Services
{
    public class NumberService : INumberService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Random _random = new();

        public NumberService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<NumberResponse> GetNumbersAsync()
        {
            var numbers = GetSessionNumbers();
            return await Task.FromResult(new NumberResponse
            {
                Numbers = numbers,
                Count = numbers.Count,
                Sum = numbers.Sum(n => n.Value)
            });
        }

        public async Task<NumberResponse> AddNumberAsync()
        {
            var numbers = GetSessionNumbers();
            var newNumber = new NumberItem
            {
                Id = numbers.Count + 1,
                Value = _random.Next(1, 101) // Random number between 1 and 100
            };
            
            numbers.Add(newNumber);
            SetSessionNumbers(numbers);

            return await Task.FromResult(new NumberResponse
            {
                Numbers = numbers,
                Count = numbers.Count,
                Sum = numbers.Sum(n => n.Value)
            });
        }

        public async Task<NumberResponse> ClearNumbersAsync()
        {
            var numbers = new List<NumberItem>();
            SetSessionNumbers(numbers);

            return await Task.FromResult(new NumberResponse
            {
                Numbers = numbers,
                Count = 0,
                Sum = 0
            });
        }

        public async Task<NumberResponse> GetSumAsync()
        {
            var numbers = GetSessionNumbers();
            return await Task.FromResult(new NumberResponse
            {
                Numbers = numbers,
                Count = numbers.Count,
                Sum = numbers.Sum(n => n.Value)
            });
        }

        private List<NumberItem> GetSessionNumbers()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
                return new List<NumberItem>();

            var numbersJson = session.GetString("Numbers");
            if (string.IsNullOrEmpty(numbersJson))
                return new List<NumberItem>();

            return System.Text.Json.JsonSerializer.Deserialize<List<NumberItem>>(numbersJson) ?? new List<NumberItem>();
        }

        private void SetSessionNumbers(List<NumberItem> numbers)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
                return;

            var numbersJson = System.Text.Json.JsonSerializer.Serialize(numbers);
            session.SetString("Numbers", numbersJson);
        }
    }
}
