using NumbersAPI.Models;

namespace NumbersAPI.Services
{
    public interface INumberService
    {
        Task<NumberResponse> GetNumbersAsync();
        Task<NumberResponse> AddNumberAsync();
        Task<NumberResponse> ClearNumbersAsync();
        Task<NumberResponse> GetSumAsync();
    }
}
