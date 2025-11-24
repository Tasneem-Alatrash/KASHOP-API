using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;

namespace KASHOP.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository  )
        {
            _categoryRepository = categoryRepository;
        }
        public CategoryResponse Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            _categoryRepository.Create(category);
            return category.Adapt<CategoryResponse>();
        }

        public List<CategoryResponse> GetAll()
        {
           var Categories = _categoryRepository.GetAll();
           return Categories.Adapt<List<CategoryResponse>>();
        }
    }
}