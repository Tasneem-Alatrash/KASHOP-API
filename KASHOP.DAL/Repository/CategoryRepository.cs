using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace KASHOP.DAL.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category Request)
        {
            _context.Add(Request);
            await _context.SaveChangesAsync();
            return Request;
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories.Include(c => c.Trinslations).Include(c=>c.User).ToListAsync();
        }
        public async Task<Category?> FindByIdAsync(int id)
        {
            return await _context.Categories.Include(c => c.Trinslations).FirstOrDefaultAsync(c =>c.Id == id);
        }

        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}