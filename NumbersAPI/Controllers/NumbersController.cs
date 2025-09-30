using Microsoft.AspNetCore.Mvc;
using NumbersAPI.Models;
using NumbersAPI.Services;

namespace NumbersAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumbersController : ControllerBase
    {
        private readonly INumberService _numberService;
        private readonly ILogger<NumbersController> _logger;

        public NumbersController(INumberService numberService, ILogger<NumbersController> logger)
        {
            _numberService = numberService ?? throw new ArgumentNullException(nameof(numberService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<NumberResponse>> GetNumbers()
        {
            try
            {
                var result = await _numberService.GetNumbersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving numbers");
                return StatusCode(500, "An error occurred while retrieving numbers");
            }
        }

        [HttpPost("add")]
        public async Task<ActionResult<NumberResponse>> AddNumber()
        {
            try
            {
                var result = await _numberService.AddNumberAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding number");
                return StatusCode(500, "An error occurred while adding number");
            }
        }

        [HttpPost("clear")]
        public async Task<ActionResult<NumberResponse>> ClearNumbers()
        {
            try
            {
                var result = await _numberService.ClearNumbersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing numbers");
                return StatusCode(500, "An error occurred while clearing numbers");
            }
        }

        [HttpGet("sum")]
        public async Task<ActionResult<NumberResponse>> GetSum()
        {
            try
            {
                var result = await _numberService.GetSumAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating sum");
                return StatusCode(500, "An error occurred while calculating sum");
            }
        }
    }
}
