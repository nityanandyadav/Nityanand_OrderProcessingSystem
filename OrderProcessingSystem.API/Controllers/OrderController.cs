using Microsoft.AspNetCore.Mvc;
using NLog;
using OrderProcessingSystem.API.DTOs;
using OrderProcessingSystem.Data.Models;
using OrderProcessingSystem.Services.Interfaces;

namespace OrderProcessingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            try
            {
                _logger.LogInformation("Received request to create a new order.");
                if (orderDto == null || orderDto.CustomerId == 0 || orderDto.ProductIds == null || !orderDto.ProductIds.Any())
                {
                    _logger.LogWarning("Invalid order data provided.");
                    return BadRequest("Invalid order data.");
                }

                var createdOrder = await _orderService.CreateOrderAsync(orderDto);
                _logger.LogInformation($"Successfully created order with ID {createdOrder.OrderId}");

                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.OrderId }, createdOrder);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating the order.");
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business rule violation while creating the order.");
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the order.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                _logger.LogInformation($"Received request to fetch order with ID {id}");
                var order = await _orderService.GetOrderByIdAsync(id);

                if (order == null)
                {
                    _logger.LogWarning($"Order with ID {id} not found.");
                    return NotFound($"Order with ID {id} not found.");
                }

                _logger.LogInformation($"Successfully fetched order with ID {id}");
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching order with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
