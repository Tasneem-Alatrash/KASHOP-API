using System;
using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace KASHOP.DAL.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    public ProductRepository(ApplicationDbContext context)
    {
      
        _context = context;
    }
      public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.Include(p=>p.Translation).Include(u=>u.User).ToListAsync();
    }
    public async Task<Product> AddAsync(Product request)
    {
        await _context.AddAsync(request);
        await _context.SaveChangesAsync();
        return request;
    }

  
}
