using NumbersAPI.Models;

namespace NumbersAPI.Tests.TestHelpers
{
    public static class TestDataBuilder
    {
        public static List<NumberItem> CreateNumberList(int count)
        {
            var numbers = new List<NumberItem>();
            for (int i = 1; i <= count; i++)
            {
                numbers.Add(new NumberItem
                {
                    Id = i,
                    Value = i * 10, // Predictable test values
                    CreatedAt = DateTime.UtcNow.AddMinutes(-i)
                });
            }
            return numbers;
        }

        public static NumberItem CreateNumberItem(int id = 1, int value = 100)
        {
            return new NumberItem
            {
                Id = id,
                Value = value,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static NumberResponse CreateNumberResponse(List<NumberItem> numbers)
        {
            return new NumberResponse
            {
                Numbers = numbers,
                Count = numbers.Count,
                Sum = numbers.Sum(n => n.Value)
            };
        }
    }
}
