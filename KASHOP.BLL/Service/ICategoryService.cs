using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;

namespace KASHOP.BLL.Service
{
    public interface ICategoryService
    {
        List<CategoryResponse> GetAll();
        CategoryResponse Create(CategoryRequest request);
    }
}