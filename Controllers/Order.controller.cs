/*
 * File: Order Controller
 * Author: Fernando B.K.M.
 * Description: This file contains the endpoints for Order  management.

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
    public class OrderController : ControllerBase
    {
        private readonly OrderServices _orderService;

        public OrderController(OrderServices orderService)
        {
            _orderService = orderService;
        }

        //! Create order
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Order order)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
                return Unauthorized(new { message = "User not authorized" });

            var response = await _orderService.Create(userId, order);
            return Ok(new ApiResponse<Order>("Create successful", response));
        }

        //! Get all orders
        [Authorize]
        [HttpGet]


        public async Task<IActionResult> GetAll()
        {
            var response = await _orderService.GetAll();
            return Ok(new ApiResponse<object>("Successful", response));
        }

        [Authorize]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingOrders()
        {
            var response = await _orderService.GetPendingOrders();
            return Ok(new ApiResponse<IEnumerable<Order>>("Pending orders retrieved successfully", response));
        }

        //! Update an order
        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateOrderDto order)
        {
            var response = await _orderService.Update(id, order);
            if (response == null)
                return NotFound(new { message = "Order not found" });

            return Ok(new ApiResponse<object>("Update successful", response));
        }




        //! Get order items by vendor ID
        [Authorize]
        [HttpGet("items/vendor/{vendorId}")]
        public async Task<IActionResult> GetOrderItemsByVendorId(string vendorId)
        {
            var response = await _orderService.GetOrderItemsByVendorId(vendorId);
            return Ok(new ApiResponse<IEnumerable<OrderItemDto>>("Order items retrieved successfully", response)); // Updated to return OrderItemDto
        }

        [Authorize]
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetOrdersByCustomerId(string customerId)
        {
            var orders = await _orderService.GetOrdersByCustomerId(customerId);

            if (orders == null || !orders.Any())
            {
                return NotFound(new ApiResponse<object>("No orders found for this customer."));
            }

            return Ok(new ApiResponse<IEnumerable<Order>>("Orders retrieved successfully.", orders));
        }

        [Authorize(Roles = "Admin, CSR")]
        [HttpDelete("customer/{customerId}")]
        public async Task<IActionResult> DeleteByCustomerId(string customerId)
        {
            var isDeleted = await _orderService.DeleteByCustomerId(customerId);

            if (!isDeleted)
            {
                return NotFound(new { message = "No orders found for the given customer." });
            }

            return Ok(new { message = "Orders deleted successfully." });
        }



        //! Delete an order
        [Authorize(Roles = "Admin, Vendor, CSR")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _orderService.Delete(id);
            if (response == null)
                return NotFound(new { message = "Order not found" });

            return Ok(new ApiResponse<Order>("Delete successful", response));
        }




        // Change the parameter to be received from query instead of body
        [Authorize(Roles = "Admin, Vendor, CSR")]
        [HttpPut("update-status/{orderId}")]
        public async Task<IActionResult> UpdateStatus(string orderId, [FromQuery] string newStatus) // Change here
        {
            var updatedOrder = await _orderService.UpdateStatus(orderId, newStatus);

            if (updatedOrder == null)
            {
                return NotFound(new ApiResponse<object>("Order not found."));
            }

            return Ok(new ApiResponse<Order>("Order status updated successfully.", updatedOrder));
        }

    }
}
