/*
 * File: Inventory Controller
 * Author: Fernando B.K.M.
 * Description: This file contains the endpoints for Inventory management.
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
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        //! Get all inventories
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _inventoryService.GetAll();
            return Ok(new ApiResponse<object>("Successful", response));
        }

        //! Create a new inventory and update product stock
        [Authorize(Roles = "Vendor, Admin, CSR")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Inventory inventory)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized(new ApiResponse<object>("User ID is missing in the token."));
            }

            try
            {
                var response = await _inventoryService.Create(userId, inventory);
                return Ok(new ApiResponse<Inventory>("Inventory created and product stock updated successfully", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>($"Error: {ex.Message}"));
            }
        }

        //! Update an inventory and modify product stock
        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateInventoryDto inventoryDto)
        {
            try
            {
                var response = await _inventoryService.Update(id, inventoryDto);
                return Ok(new ApiResponse<object>("Inventory updated successfully", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>($"Error: {ex.Message}"));
            }
        }

        //! Get inventories by product ID
        [Authorize]
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(string productId)
        {
            var inventories = await _inventoryService.GetByProductId(productId);
            if (inventories == null || !inventories.Any())
            {
                return NotFound(new ApiResponse<object>("No inventories found for this product."));
            }

            return Ok(new ApiResponse<IEnumerable<Inventory>>("Inventories retrieved successfully.", inventories));
        }


        // GET api/inventory/vendor/{vendorId}
        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetInventoriesByVendorId(string vendorId)
        {
            var inventories = await _inventoryService.GetByVendorId(vendorId);
            if (inventories == null || !inventories.Any())
            {
                return NotFound(new { Message = "No inventories found for this vendor." });
            }

            return Ok(new ApiResponse<IEnumerable<Inventory>>("Inventories retrieved successfully.", inventories));
        }


        //! Delete an inventory
        [Authorize(Roles = "Vendor, Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var response = await _inventoryService.Delete(id);
                return Ok(new ApiResponse<Inventory>("Inventory deleted successfully", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>($"Error: {ex.Message}"));
            }
        }
    }
}
