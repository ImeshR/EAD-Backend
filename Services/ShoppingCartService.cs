/*
 * File: Shopping Cart Service
 * Author: Fernando B.K.M.
 * Description: This file contains the business logic for Shopping Cart management.
*/
using EAD_Backend.Data;
using EAD_Backend.DTOs;
using EAD_Backend.Models;
using MongoDB.Driver;

namespace EAD_Backend.Services
{
    public class ShoppingCartService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<ShoppingCart> _shoppingCartModel;

        public ShoppingCartService(MongoDBService mongoDbService, IConfiguration configuration)
        {
            _configuration = configuration;
            _shoppingCartModel = mongoDbService.Database?.GetCollection<ShoppingCart>("shoppingCarts");
        }

        // Get all shopping carts
        public async Task<IEnumerable<ShoppingCart>> GetAll()
        {
            return await _shoppingCartModel.Find(cart => true).ToListAsync();
        }

        // Create a shopping cart
        public async Task<ShoppingCart> Create(string customerId, ShoppingCart shoppingCart)
        {
            shoppingCart.CustomerId = customerId;
            shoppingCart.CreatedAt = DateTime.UtcNow;
            shoppingCart.UpdatedAt = DateTime.UtcNow;
            await _shoppingCartModel.InsertOneAsync(shoppingCart);
            return shoppingCart;
        }

        // Update a shopping cart
        public async Task<object> Update(string id, UpdateShoppingCartDto shoppingCartDto)
        {
            var filter = Builders<ShoppingCart>.Filter.Eq("Id", id);
            var update = Builders<ShoppingCart>.Update
                .Set("customerId", shoppingCartDto.CustomerId)
                .Set("items", shoppingCartDto.Items)
                .Set("updatedAt", DateTime.UtcNow);

            await _shoppingCartModel.UpdateOneAsync(filter, update);
            return shoppingCartDto;
        }

        // Get shopping cart by CustomerId
        public async Task<ShoppingCart?> GetByCustomerId(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                throw new ArgumentException("Customer ID cannot be null or empty.");
            }

            var filter = Builders<ShoppingCart>.Filter.Eq("CustomerId", customerId);
            return await _shoppingCartModel.Find(filter).FirstOrDefaultAsync();
        }

        // Delete a shopping cart
        public async Task<ShoppingCart> Delete(string id)
        {
            var filter = Builders<ShoppingCart>.Filter.Eq("Id", id);
            return await _shoppingCartModel.FindOneAndDeleteAsync(filter);
        }



        // Delete all cart items by customer ID
        public async Task<bool> DeleteCartItemsByCustomerId(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer ID cannot be null or empty.");

            var filter = Builders<ShoppingCart>.Filter.Eq(cart => cart.CustomerId, customerId);

            // Update the cart by setting the Items array to an empty list
            var update = Builders<ShoppingCart>.Update.Set(cart => cart.Items, new List<CartItem>());

            var result = await _shoppingCartModel.UpdateOneAsync(filter, update);

            // Check if any document was modified
            return result.ModifiedCount > 0;
        }
    }
}
