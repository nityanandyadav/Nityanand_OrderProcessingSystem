using OrderProcessingSystem.API.DTOs;
using OrderProcessingSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessingSystem.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(OrderCreateDto order);
    }
}
