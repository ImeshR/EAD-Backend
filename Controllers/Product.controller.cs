/*
 * File: Product Controller
 * Author: Fernando B.K.M.
 * Description: This file contains the endpoints for Product management.

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
    public class ProductController : ControllerBase
    {

        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        //! ======================================================== Define API Endpoints ============================================================>

        //! Get all products
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var response = await _productService.GetAll();
            return Ok(new ApiResponse<object>("Successful", response));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound(new ApiResponse<object>("Product not found."));
            }

            return Ok(new ApiResponse<Product>("Product retrieved successfully.", product));
        }


        //! Create a product
        [Authorize(Roles = "Vendor, Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(Product product)
        {
            //var userId = User.FindFirst("UserId")?.Value;
            var response = await _productService.Create(product);
            return Ok(new ApiResponse<Product>("Create successful", response));
        }


        //! Get products by category ID
        [Authorize]
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategoryId(string categoryId)
        {
            var products = await _productService.GetByCategoryId(categoryId);
            if (!products.Any())
            {
                return NotFound(new ApiResponse<object>("No products found for this category."));
            }
            return Ok(new ApiResponse<IEnumerable<Product>>("Products retrieved successfully.", products));
        }

        //! Get products by vendor ID
        [Authorize]
        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetByVendorId(string vendorId)
        {
            var response = await _productService.GetByVendorId(vendorId);
            return Ok(new ApiResponse<IEnumerable<Product>>("Products retrieved successfully", response));
        }



        //! Update a product
        [Authorize(Roles = "Vendor, Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, UpdateProductDto product)
        {

            var response = await _productService.Update(id, product);
            return Ok(new ApiResponse<object>("Update successful", response));
        }

        //! Delete a product
        [Authorize(Roles = "Vendor, Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _productService.Delete(id);
            return Ok(new ApiResponse<Product>("Delete successful", response));
        }
    }
}


