/*
 * File: Payment Controller
 * Author: Fernando B.K.M.
 * Description: This file contains the endpoints for Payment management.
 */

using EAD_Backend.Models;
using EAD_Backend.OtherModels;
using EAD_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EAD_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        //! ======================================================== Define API Endpoints ============================================================> 

        //! Get all payments
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _paymentService.GetAll();
            return Ok(new ApiResponse<object>("Successful", response));
        }

        //! Get payment by ID
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _paymentService.GetById(id);
            if (response == null)
                return NotFound(new ApiResponse<object>("Payment not found", null));

            return Ok(new ApiResponse<Payment>("Successful", response));
        }

        //! Create a new payment
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create(Payment payment)
        {
            var response = await _paymentService.Create(payment);
            return Ok(new ApiResponse<Payment>("Create successful", response));
        }

        //! Update payment status
        [Authorize(Roles = "Admin")]
        [HttpPut("update-status/{id}")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] string newStatus)
        {
            var response = await _paymentService.UpdatePaymentStatus(id, newStatus);
            if (response == null)
                return NotFound(new ApiResponse<object>("Payment not found", null));

            return Ok(new ApiResponse<Payment>("Update successful", response));
        }

        //! Delete a payment
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _paymentService.Delete(id);
            if (response == null)
                return NotFound(new ApiResponse<object>("Payment not found", null));

            return Ok(new ApiResponse<Payment>("Delete successful", response));
        }
    }
}
