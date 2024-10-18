/*
 * File: Order Tracking Controller
 * Author: Fernando B.K.M.
 * Description: This file contains the endpoints for Order Tracking   management.

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

    public class OrderTrackingController : ControllerBase

    {
        private readonly OrderTrackingService _orderTrackingService;

        public OrderTrackingController(OrderTrackingService orderTrackingService)
        {
            _orderTrackingService = orderTrackingService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var response = await _orderTrackingService.GetAll();
            return Ok(new ApiResponse<object>("Successful", response));
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(OrderTracking orderTracking)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _orderTrackingService.Create(userId, orderTracking);
            return Ok(new ApiResponse<OrderTracking>("Create successful", response));
        }



        [Authorize(Roles = "Vendor, Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _orderTrackingService.Delete(id);
            return Ok(new ApiResponse<OrderTracking>("Delete successful", response));
        }

    }

}
