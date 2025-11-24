using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KASHOP.DAL.Models;

namespace KASHOP.DAL.Repository
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category Create(Category Request);
    }
}