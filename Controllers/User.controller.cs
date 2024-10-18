/*
 * File: User Controller
 * Author: Fernando B.K.M.
 * Description: This file contains the endpoints for User  management.

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
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        //! ======================================================== Define API Endpoints ============================================================>

        //! Get all users
        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _userService.GetAll();
            return Ok(new ApiResponse<object>("Successful", response));
        }

        //! Register a user
        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Create(User user)
        {

            var response = await _userService.Create(user);
            return Ok(new ApiResponse<User>("Create successful", response));
        }

        //! Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            var response = await _userService.Login(user);
            return Ok(new ApiResponse<object>("Login successful", response));
        }

        //! Self Register
        [HttpPost("self-register")]
        public async Task<IActionResult> SelfRegister(SelfRegisterDto user)
        {
            var response = await _userService.SelfRegister(user);
            return Ok(new ApiResponse<User>("Create successful", response));
        }

        //! Activate account
        [Authorize(Roles = "CSR")]
        [HttpGet("activate/{id}")]
        public async Task<IActionResult> ActivateAccount(string id)
        {
            var response = await _userService.ActivateAccount(id);
            return Ok(new ApiResponse<object>("Account activated", response));
        }

        //! Deactivate account
        [Authorize]
        [HttpGet("deactivate/{id}")]
        public async Task<IActionResult> DeactivateAccount(string id)
        {
            var response = await _userService.DeactivateAccount(id);
            return Ok(new ApiResponse<object>("Account deactivated", response));
        }

        //! Update user

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateUserDto user)
        {
            var response = await _userService.Update(id, user);
            return Ok(new ApiResponse<object>("Update successful", response));
        }



        //! Get a vendor by ID
        [Authorize(Roles = "Admin")]
        [HttpGet("vendor/{id}")]
        public async Task<IActionResult> GetVendorById(string id)
        {


            var response = await _userService.GetVendorById(id);
            return Ok(new ApiResponse<User>("Vendor retrieved successfully", response));


        }


        //! Get all vendors
        [Authorize(Roles = "Admin , Vendor")]
        [HttpGet("vendors")]
        public async Task<IActionResult> GetAllVendors()
        {
            var response = await _userService.GetAllVendors();
            return Ok(new ApiResponse<object>("Vendors retrieved successfully", response));
        }

        //! Get all admins
        [Authorize(Roles = "Admin")]
        [HttpGet("admins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            try
            {
                var response = await _userService.GetAllAdmins();
                return Ok(new ApiResponse<object>("Admins retrieved successfully", response));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiResponse<object>(ex.Message, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("An error occurred", ex.Message));
            }
        }






        //! Get all CSRs
        [Authorize(Roles = "Admin")]
        [HttpGet("csrs")]
        public async Task<IActionResult> GetAllCSRs()
        {
            try
            {
                var response = await _userService.GetAllCSRs();
                return Ok(new ApiResponse<object>("CSRs retrieved successfully", response));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiResponse<object>(ex.Message, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>("An error occurred", ex.Message));
            }
        }

        // Get user by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(new ApiResponse<User>("User retrieved successfully", user));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }







    }
}