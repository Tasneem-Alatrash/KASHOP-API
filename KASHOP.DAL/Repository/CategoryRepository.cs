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

        public Category Create(Category Request)
        {
            _context.Add(Request);
            _context.SaveChanges();
            return Request;
        }

        public List<Category> GetAll()
        {
            return _context.Categories.Include(c => c.Trinslations).ToList();
        }
    }
}