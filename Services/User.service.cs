/*
 * File: User Service
 * Author: Fernando B.K.M.
 * Description: This file contains the business logic for User management.
*/
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EAD_Backend.Data;
using EAD_Backend.DTOs;
using EAD_Backend.Models;
using EAD_Backend.OtherModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace EAD_Backend.Services
{

    public class UserService
    {

        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<User> _userModel;
        private readonly IMongoCollection<Role> _roleModel;
        private readonly NotificationService _notificationService;





        public UserService(MongoDBService mongoDbService, IConfiguration configuration, NotificationService notificationService)
        {
            _configuration = configuration;
            _userModel = mongoDbService.Database?.GetCollection<User>("users");
            _roleModel = mongoDbService.Database?.GetCollection<Role>("roles");
            _notificationService = notificationService;

        }

        //! =======================================================  Define Business | DB Operations for User ===================================>
        //! Get all users
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userModel.Find(user => true).ToListAsync();
        }

        //! create a user
        public async Task<User> Create(User user)
        {
            user.Active = true;
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            await _userModel.InsertOneAsync(user);
            return user;
        }


        //! Login
        public async Task<object> Login(LoginDto user)
        {
            var userInDb = await _userModel.Find(u => u.Email == user.Email).FirstOrDefaultAsync();
            if (userInDb == null)
            {
                throw new ArgumentException("User not found");

            }

            // Verify the hashed password
            var response = _passwordHasher.VerifyHashedPassword(userInDb, userInDb.Password, user.Password);

            if (response == PasswordVerificationResult.Failed)
            {

                throw new UnauthorizedAccessException("Invalid password");
            }


            var role = await _roleModel.Find(r => r.Id == userInDb.Role).FirstOrDefaultAsync();

            if (role == null)
            {
                throw new ArgumentException("Role not found");
            }



            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", userInDb.Id.ToString()),
                new Claim("Email", userInDb.Email.ToString()),
                new Claim(ClaimTypes.Role, role.Name),



            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: signin
            );

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return new
            {
                token = tokenValue,
                role = role.Name,
                name = userInDb.Name,
                email = userInDb.Email,
                id = userInDb.Id

            }; ;

        }

        // Get a user by ID
public async Task<User> GetUserById(string id)
{
    if (string.IsNullOrEmpty(id))
    {
        throw new ArgumentException("User ID cannot be null or empty.");
    }

    var user = await _userModel.Find(u => u.Id == id).FirstOrDefaultAsync();

    if (user == null)
    {
        throw new ArgumentException("User not found");
    }

    return user;
}






        //! Self Register
        public async Task<User> SelfRegister(SelfRegisterDto user)
        {
            // Check if the user already exists
            var userInDb = await _userModel.Find(u => u.Email == user.Email).FirstOrDefaultAsync();
            if (userInDb != null)
            {
                throw new ArgumentException("User email already exists");
            }

            // Assign the 'Customer' role to the user
            var role = await _roleModel.Find(r => r.Name == "Customer").FirstOrDefaultAsync();
            if (role == null)
            {
                throw new ArgumentException("Role not found");
            }

            // Convert to User object
            User newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = role.Id,
                Active = false
            };

            // Hash the password
            newUser.Password = _passwordHasher.HashPassword(newUser, user.Password);

            // Insert the new user into the database
            await _userModel.InsertOneAsync(newUser);

            // Notify the admin about the new registration
            var adminId = _configuration["Admin:Id"];
            var notification = new Notification
            {
                UserId = adminId,
                Message = $"A new user '{newUser.Name}' has registered.",
                Type = "NewUserRegistration",
                ReadStatus = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationService.Create(adminId, notification);

            return newUser;
        }



        //! Activate account
        public async Task<object> ActivateAccount(string id)
        {
            var user = await _userModel.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            user.Active = true;
            var response = await _userModel.ReplaceOneAsync(u => u.Id == id, user);
            return response;
        }


        //! Deactivate account
        public async Task<object> DeactivateAccount(string id)
        {
            var user = await _userModel.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            user.Active = false;
            var response = await _userModel.ReplaceOneAsync(u => u.Id == id, user);
            return response;
        }

        //! Update user
        public async Task<User> Update(string id, UpdateUserDto userDto)
        {
            // Find the existing user in the database
            var userInDb = await _userModel.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (userInDb == null)
            {
                throw new ArgumentException("User not found");
            }

            // Update the user properties with values from the DTO
            userInDb.Name = userDto.Name;
            userInDb.Role = userDto.Role;
            userInDb.Active = userDto.Active;




            // Update the existing user in the database
            var response = await _userModel.ReplaceOneAsync(u => u.Id == id, userInDb);

            // Check if the update was successful
            if (!response.IsAcknowledged || response.ModifiedCount == 0)
            {
                throw new Exception("Failed to update the user");
            }

            // Return the updated user
            return userInDb;
        }

        //! Get all vendors
        public async Task<IEnumerable<User>> GetAllVendors()
        {
            // Find the 'Vendor' role from the roles collection
            var vendorRole = await _roleModel.Find(r => r.Name == "Vendor").FirstOrDefaultAsync();
            if (vendorRole == null)
            {
                throw new ArgumentException("Vendor role not found");
            }

            // Retrieve all users with the 'Vendor' role ID
            var vendors = await _userModel.Find(user => user.Role == vendorRole.Id).ToListAsync();
            return vendors;
        }



        //! Get all admins
        public async Task<IEnumerable<User>> GetAllAdmins()
        {
            // Find the 'Admin' role from the roles collection
            var adminRole = await _roleModel.Find(r => r.Name == "Admin").FirstOrDefaultAsync();
            if (adminRole == null)
            {
                throw new ArgumentException("Admin role not found");
            }

            // Retrieve all users with the 'Admin' role ID
            var admins = await _userModel.Find(user => user.Role == adminRole.Id).ToListAsync();
            return admins;
        }


        //! Get all CSRs
        public async Task<IEnumerable<User>> GetAllCSRs()
        {
            // Find the 'CSR' role from the roles collection
            var csrRole = await _roleModel.Find(r => r.Name == "CSR").FirstOrDefaultAsync();
            if (csrRole == null)
            {
                throw new ArgumentException("CSR role not found");
            }

            // Retrieve all users with the 'CSR' role ID
            var csrs = await _userModel.Find(user => user.Role == csrRole.Id).ToListAsync();
            return csrs;
        }




        //! Get a vendor by ID
        public async Task<User> GetVendorById(string id)
        {
            // Find the user by ID
            var user = await _userModel.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("Vendor not found");
            }

            // Verify if the user has the 'Vendor' role
            var role = await _roleModel.Find(r => r.Id == user.Role).FirstOrDefaultAsync();
            if (role == null || role.Name != "Vendor")
            {
                throw new UnauthorizedAccessException("User is not a vendor");
            }

            return user;
        }

    }

}