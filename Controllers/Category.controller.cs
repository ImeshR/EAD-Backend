/*
 * File: Category Controller
 * Author: Fernando B.K.M.
 * Description: This file contains the endpoints for Category management.
 */

using EAD_Backend.DTOs;
using EAD_Backend.Models;
using EAD_Backend.OtherModels;
using EAD_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EAD_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize]
        [HttpGet]
        //! Get all categories
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAll();
            return Ok(new ApiResponse<object>("Successful", response));
        }

        [Authorize(Roles = "Vendor, Admin")]
        [HttpPost("create")]
        //! Create a new category
        public async Task<IActionResult> Create(Category category)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _categoryService.Create(userId, category);
            return Ok(new ApiResponse<Category>("Create successful", response));
        }

        [Authorize]
        [HttpPut("update/{id}")]
        //! Update a category
        public async Task<IActionResult> Update(string id, UpdateCategoryDto category)
        {
            var response = await _categoryService.Update(id, category);
            return Ok(new ApiResponse<object>("Update successful", response));
        }

        [Authorize(Roles = "Vendor, Admin")]
        [HttpDelete("delete/{id}")]
        //! Delete a category
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _categoryService.Delete(id);
            return Ok(new ApiResponse<Category>("Delete successful", response));
        }


        //! Get category by ID
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound(new ApiResponse<object>("Category not found."));
            }

            return Ok(new ApiResponse<Category>("Category retrieved successfully.", category));
        }

        [Authorize]
        [HttpGet("active")]
        //! Get all active categories (status = "true")
        public async Task<IActionResult> GetActiveCategories()
        {
            var response = await _categoryService.GetActiveCategories();
            return Ok(new ApiResponse<object>("Successful", response));
        }
    }
}
