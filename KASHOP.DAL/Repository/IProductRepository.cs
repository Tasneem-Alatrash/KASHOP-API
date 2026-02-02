using System;
using KASHOP.DAL.Models;

namespace KASHOP.DAL.Repository;

public interface IProductRepository
{
        
    Task<List<Product>> GetAllAsync();

    Task<Product> AddAsync(Product request);

}
