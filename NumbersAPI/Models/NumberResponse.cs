namespace NumbersAPI.Models
{
    public class NumberResponse
    {
        public List<NumberItem> Numbers { get; set; } = new();
        public int Count { get; set; }
        public int Sum { get; set; }
    }
}
