using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.Data;
using OrderProcessingSystem.Data.Models;
using OrderProcessingSystem.Repositories.Interfaces;
using NLog;
using System;
using Microsoft.Extensions.Logging;
public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepository(ApplicationDbContext context, ILogger<CustomerRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        try
        {
            return await _context.Customers.Include(c => c.Orders).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all customers");
            throw;
        }
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        try
        {
            return await _context.Customers.Include(c => c.Orders)
                                           .ThenInclude(o => o.Products)
                                           .FirstOrDefaultAsync(c => c.CustomerId == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching customer by ID {id}", id);
            throw;
        }
    }
}
