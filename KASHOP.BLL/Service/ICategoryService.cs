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
        Task<List<CategoryResponse>> GetAll();
         Task<CategoryResponse> CreateAsync(CategoryRequest request);
        Task<BaseResponse> DeleteCategoryAsync(int id);
        Task<BaseResponse> UpdateCategory(int id , CategoryRequest request);
        Task<BaseResponse> ToggleStatus(int id);
    }
}