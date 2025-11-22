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


namespace KASHOP.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public CategoriesController(ApplicationDbContext context , IStringLocalizer<SharedResources> localizer)
        {
            _context = context;
            _localizer = localizer;
        }
        [HttpGet("")]
        public IActionResult index()
        {
            var Categories = _context.Categories.Include(c => c.Trinslations).ToList();
            var response = Categories.Adapt<List<CategoryResponse>>();
            return Ok(new { Message = _localizer["Success"].Value,  response });
        }
        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok(new { Message = _localizer["Success"].Value });
        }
    }
}