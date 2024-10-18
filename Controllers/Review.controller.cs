/*
 * File: Review Controller
 * Author: Fernando B.K.M.
 * Description: This file contains the endpoints for Review management.
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
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize]
        [HttpGet]
        //! Get all reviews
        public async Task<IActionResult> GetAll()
        {
            var response = await _reviewService.GetAll();
            return Ok(new ApiResponse<object>("Successful", response));
        }

        // Get reviews by VendorId
        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetByVendorId(string vendorId)
        {
            var response = await _reviewService.GetByVendorId(vendorId);
            return Ok(new ApiResponse<IEnumerable<Review>>("Retrieved successfully", response));
        }

        [Authorize]
        [HttpPost("create")]
        //! Create a new review
        public async Task<IActionResult> Create(Review review)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _reviewService.Create(userId, review);
            return Ok(new ApiResponse<Review>("Create successful", response));
        }

        [Authorize(Roles = "Vendor, Admin")]
        [HttpDelete("delete/{id}")]
        //! Delete a review
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _reviewService.Delete(id);
            return Ok(new ApiResponse<Review>("Delete successful", response));
        }

        // Get replies for a specific review
        [HttpGet("reply/{reviewId}")]
        public async Task<IActionResult> GetReplies(string reviewId)
        {
            var response = await _reviewService.GetReplies(reviewId);
            return Ok(new ApiResponse<List<Reply>>("Replies retrieved successfully", response));
        }

        // Add a reply to a review using PUT
        [Authorize(Roles = "Vendor, Admin")]
        [HttpPut("reply/{reviewId}")]
        public async Task<IActionResult> AddReply(string reviewId, [FromBody] Reply reply)
        {

            var response = await _reviewService.AddReply(reviewId, reply);
            return Ok(new ApiResponse<Review>("Reply added successfully", response));

        }
    }
}
