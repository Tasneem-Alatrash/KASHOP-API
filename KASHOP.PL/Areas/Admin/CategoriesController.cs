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
    [Authorize]
    public class CategoriesController : ControllerBase
    {
         private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public CategoriesController(ICategoryService categoryService , IStringLocalizer<SharedResources> localizer)
        {
            _categoryService = categoryService;
            _localizer = localizer;
        }

         [HttpPost("")]
        public IActionResult Create(CategoryRequest Request)
        {
            var response = _categoryService.Create(Request);
            return Ok(new
            {
                Message = _localizer["Success"].Value
            });
        }
    
        
    }
}
