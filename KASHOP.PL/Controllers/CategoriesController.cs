using System;
using KASHOP.DAL;
using KASHOP.PL.Resources;
using Mapster;
using KASHOP.DAL.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using KASHOP.DAL.Models;
using KASHOP.DAL.DTO.Request;
using Microsoft.EntityFrameworkCore;
using KASHOP.DAL.Repository;
using KASHOP.BLL.Service;


namespace KASHOP.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {

        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICategoryService _categoryService;
        public CategoriesController(IStringLocalizer<SharedResources> localizer , ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _localizer = localizer;
        }
        [HttpGet("")]
        public IActionResult index()
        {
            var response = _categoryService.GetAll();
            return Ok(new { Message = _localizer["Success"].Value,  response });
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            _categoryService.Create(request);
            return Ok(new { Message = _localizer["Success"].Value });
        }
    }
}