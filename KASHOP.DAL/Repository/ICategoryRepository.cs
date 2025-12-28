using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KASHOP.DAL.Models;

namespace KASHOP.DAL.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAll();
        Task<Category> CreateAsync(Category Request);
        Task<Category?> FindByIdAsync(int id);
        Task DeleteAsync(Category category);
        Task<Category?> UpdateAsync(Category category);
    }
}