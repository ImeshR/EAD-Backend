/*
 * File: Shopping Cart Controller
 * Author: Fernando B.K.M.
 * Description: This file contains the endpoints for Shopping Cart management.

*/using EAD_Backend.DTOs;
using EAD_Backend.Models;
using EAD_Backend.OtherModels;
using EAD_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EAD_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartController(ShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        // Get all shopping carts
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _shoppingCartService.GetAll();
            return Ok(new ApiResponse<object>("Successful", response));
        }

        // Create a shopping cart
        [Authorize(Roles = "Customer, Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(ShoppingCart shoppingCart)
        {
            var customerId = User.FindFirst("UserId")?.Value;
            var response = await _shoppingCartService.Create(customerId, shoppingCart);
            return Ok(new ApiResponse<ShoppingCart>("Create successful", response));
        }

        // Update a shopping cart
        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, UpdateShoppingCartDto shoppingCartDto)
        {
            var response = await _shoppingCartService.Update(id, shoppingCartDto);
            return Ok(new ApiResponse<object>("Update successful", response));
        }

        // Delete a shopping cart
        [Authorize(Roles = "Customer, Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _shoppingCartService.Delete(id);
            return Ok(new ApiResponse<ShoppingCart>("Delete successful", response));
        }


        // Get shopping cart by CustomerId
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(string customerId)
        {
            var response = await _shoppingCartService.GetByCustomerId(customerId);

            if (response == null)
            {
                return NotFound(new ApiResponse<ShoppingCart>("Shopping cart not found."));
            }

            return Ok(new ApiResponse<ShoppingCart>("Retrieved successfully", response));
        }
[Authorize(Roles = "Customer, Admin")]
[HttpDelete("customer/{customerId}/items")]
public async Task<IActionResult> DeleteCartItemsByCustomerId(string customerId)
{
    var isDeleted = await _shoppingCartService.DeleteCartItemsByCustomerId(customerId);

    if (!isDeleted)
    {
        return NotFound(new { message = "No shopping cart found for the given customer or no items to delete." });
    }

    return Ok(new { message = "Cart items deleted successfully." });
}


    }
}
