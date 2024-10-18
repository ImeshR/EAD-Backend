/*
 * File: Notification Controller
 * Author: Fernando B.K.M.
 * Description: This file contains the endpoints for Notification  management.

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

    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }



        [Authorize]
        [HttpGet]

        //! Get all notifications
        public async Task<IActionResult> GetAll()
        {

            var response = await _notificationService.GetAll();
            return Ok(new ApiResponse<object>("Successful", response));
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetByType(string type)
        {
            var notifications = await _notificationService.GetByType(type);
            return Ok(notifications);
        }
        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var notifications = await _notificationService.GetByUserId(userId);

            if (notifications == null || !notifications.Any())
            {
                return NotFound(new ApiResponse<object>("No notifications found for this user."));
            }

            return Ok(new ApiResponse<IEnumerable<Notification>>("Notifications retrieved successfully.", notifications));
        }


        [Authorize(Roles = "Vendor, Admin")]
        [HttpPost("create")]


        //! Create a new notification
        public async Task<IActionResult> Create(Notification notification)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _notificationService.Create(userId, notification);
            return Ok(new ApiResponse<Notification>("Create successful", response));
        }




        [Authorize]
        [HttpPut("update/{id}")]
        //! Update a notification
        public async Task<IActionResult> Update(string id, UpdateNotificationDto notification)
        {

            var response = await _notificationService.Update(id, notification);
            return Ok(new ApiResponse<object>("Update successful", response));
        }



        [Authorize(Roles = "Vendor, Admin")]
        [HttpDelete("delete/{id}")]
        //! Delete a notification
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _notificationService.Delete(id);
            return Ok(new ApiResponse<Notification>("Delete successful", response));
        }



        [Authorize]
        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteByUserId(string userId)
        {
            var isDeleted = await _notificationService.DeleteByUserId(userId);

            if (!isDeleted)
            {
                return NotFound(new { message = "No notifications found for the given user." });
            }

            return Ok(new { message = "Notifications deleted successfully." });
        }


        [Authorize]
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteByIdAndUserId(string notificationId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
                return Unauthorized(new { message = "User not authorized" });

            var deletedNotification = await _notificationService.DeleteByIdAndUserId(notificationId, userId);

            if (deletedNotification == null)
            {
                return NotFound(new { message = "Notification not found for this user." });
            }

            return Ok(new { message = "Notification deleted successfully.", notification = deletedNotification });
        }



    }

}