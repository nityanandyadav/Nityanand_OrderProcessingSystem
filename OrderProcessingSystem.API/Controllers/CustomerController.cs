using Microsoft.AspNetCore.Mvc;
using NLog;
using OrderProcessingSystem.Services.Interfaces;

namespace OrderProcessingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        // Endpoint to get all customers
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                _logger.LogInformation("Received request to fetch all customers.");
                var customers = await _customerService.GetAllCustomersAsync();

                if (customers == null || customers.Count == 0)
                {
                    _logger.LogWarning("No customers found.");
                    return NotFound("No customers found.");
                }

                _logger.LogInformation($"Successfully fetched {customers.Count} customers.");
                return Ok(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all customers.");
                return StatusCode(500, "Internal server error");
            }
        }

        // Endpoint to get a customer by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                _logger.LogInformation($"Received request to fetch customer with ID {id}");
                var customer = await _customerService.GetCustomerByIdAsync(id);

                if (customer == null)
                {
                    _logger.LogWarning($"Customer with ID {id} not found.");
                    return NotFound($"Customer with ID {id} not found.");
                }

                _logger.LogInformation($"Successfully fetched customer with ID {id}");
                return Ok(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching customer with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
