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
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<CategoryResponse> CreateAsync(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            await _categoryRepository.CreateAsync(category);
            return category.Adapt<CategoryResponse>();
        }

        public async Task<List<CategoryResponse>> getAllCategoriesForAdimn()
        {
            var Categories = await _categoryRepository.GetAll();
            return Categories.Adapt<List<CategoryResponse>>();
        }
        public async Task<BaseResponse> DeleteCategoryAsync(int id)
        {
            try
            {
                var Category = await _categoryRepository.FindByIdAsync(id);
                if (Category is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Category Not Found",

                    };
                }
                await _categoryRepository.DeleteAsync(Category);
                return new BaseResponse
                {
                    Success = true,
                    Message = "Category Deleted Succesfully",

                };

            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Can't Delete Category",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<BaseResponse> UpdateCategory(int id, CategoryRequest request)
        {
            try
            {
                var Category = await _categoryRepository.FindByIdAsync(id);
                if (Category is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Category Not Found"

                    };
                }
                if (request.Trinslations != null)
                {
                    foreach (var Trinslations in request.Trinslations)
                    {
                        var existing = Category.Trinslations.FirstOrDefault(t => t.Language == Trinslations.Language);
                        if (existing is not null)
                        {
                            existing.Name = Trinslations.Name;
                        }
                        else
                        {
                            return new BaseResponse
                            {
                                Success = false,
                                Message = $"Language {Trinslations.Language} not suported "
                            };
                        }
                    }
                }
                await _categoryRepository.UpdateAsync(Category);
                return new BaseResponse
                {
                    Success = true,
                    Message = "Category Updated Succesuflly"

                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Can't Update Category",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        public async Task<BaseResponse> ToggleStatus(int id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Category Not Found"

                    };


                }

                category.status = category.status == Status.Active ? Status.InActice : Status.Active;
                await _categoryRepository.UpdateAsync(category);
                return new BaseResponse
                {
                    Success = true,
                    Message = "Category Cahnge Status Succesuflly"

                };


            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Can't be updated status",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<List<CategoryUserResponse>> getAllCategoriesForUser(string lang="en")
        {
            
            var categories = await _categoryRepository.GetAll();
            foreach(var category in categories)
            {
                category.Trinslations = category.Trinslations.Where(t => t.Language == lang).ToList();

            }
            var response = categories.BuildAdapter().AddParameters("lang" , lang).AdaptToType<List<CategoryUserResponse>>();
            return response;
           
        }
    }
}