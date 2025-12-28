using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
namespace KASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class CategoriesController : ControllerBase
    {
         private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public CategoriesController(ICategoryService categoryService , IStringLocalizer<SharedResources> localizer)
        {
            _categoryService = categoryService;
            _localizer = localizer;
        }

[HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var response = await _categoryService.getAllCategoriesForAdimn();
            return Ok(new
            {
                Message = _localizer["Success"].Value,
                response
            });
        }
         [HttpPost("")]
        public async Task<IActionResult> Create(CategoryRequest Request)
        {
            var response =await _categoryService.CreateAsync(Request);
            return Ok(new
            {
                Message = _localizer["Success"].Value
            });
        }
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var response = await _categoryService.DeleteCategoryAsync(id);
            if (!response.Success)
            {
                if(response.Message.Contains("Not Found"))
                {
                    return NotFound(response);
                }
                return BadRequest(response);
            }
            return Ok(response);
        }
        
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id , [FromBody] CategoryRequest category)
        {
            var response = await _categoryService.UpdateCategory(id,category);
            if (!response.Success)
            {
                if(response.Message.Contains("Not Found"))
                {
                    return NotFound(response);
                }
                return BadRequest(response);
            }
            return Ok(response);
        }
    
        [HttpPatch("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id)
        {
            var response = await _categoryService.ToggleStatus(id);
            if (!response.Success)
            {
                if(response.Message.Contains("Not Found"))
                {
                    return NotFound(response);
                }
                return BadRequest(response);
            }
            return Ok(response);
        }
        
    }
}
