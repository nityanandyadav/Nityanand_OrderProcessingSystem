using Microsoft.Extensions.Logging;
using NLog;
using OrderProcessingSystem.API.DTOs;
using OrderProcessingSystem.Data.Models;
using OrderProcessingSystem.Repositories.Interfaces;
using OrderProcessingSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingSystem.Services
{
    public class OrderService : IOrderService
    {
      private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IProductRepository productRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(OrderCreateDto orderDto)
        {
            try
            {
                // Retrieve the customer
                var customer = await _customerRepository.GetCustomerByIdAsync(orderDto.CustomerId);
                if (customer == null)
                {
                    throw new ArgumentException("Customer not found.");
                }

                // Check if the customer has any unfulfilled orders
                var unfulfilledOrder = customer.Orders.FirstOrDefault(o => !o.IsFulfilled);
                if (unfulfilledOrder != null)
                {
                    throw new InvalidOperationException("Cannot place a new order until the previous order is fulfilled.");
                }

                // Retrieve products by IDs
                var products = await _productRepository.GetProductsByIdsAsync(orderDto.ProductIds);
                if (products == null || products.Count != orderDto.ProductIds.Count)
                {
                    throw new ArgumentException("One or more product IDs are invalid.");
                }

                // Calculate total price
                var totalPrice = products.Sum(p => p.Price);

                // Create the order
                var newOrder = new Order
                {
                    CustomerId = orderDto.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    Products = products,
                    IsFulfilled = false
                };

                return await _orderRepository.CreateOrderAsync(newOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the order.");
                throw;
            }
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            try
            {
                return await _orderRepository.GetOrderByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving order with ID {id}");
                throw;
            }
        }
    }
}
