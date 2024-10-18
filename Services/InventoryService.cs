/*
 * File: Inventory Service
 * Author: Fernando B.K.M.
 * Description: This file contains the business logic for Inventory management.
 */

using EAD_Backend.Data;
using EAD_Backend.DTOs;
using EAD_Backend.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAD_Backend.Services
{
    public class InventoryService
    {
        private readonly IMongoCollection<Inventory> _inventoryModel;
        private readonly IMongoCollection<Product> _productModel;

        public InventoryService(MongoDBService mongoDbService)
        {
            _inventoryModel = mongoDbService.Database?.GetCollection<Inventory>("inventories");
            _productModel = mongoDbService.Database?.GetCollection<Product>("products");
        }

        //! Get all inventories
        public async Task<IEnumerable<Inventory>> GetAll()
        {
            return await _inventoryModel.Find(inventory => true).ToListAsync();
        }

        //! Create inventory and update product stock count
        public async Task<Inventory> Create(string userId, Inventory inventory)
        {
            inventory.VendorId = userId;
            await _inventoryModel.InsertOneAsync(inventory);

            var updateSuccess = await UpdateProductStock(inventory.ProductId, inventory.QuantityAvailable);
            if (!updateSuccess)
            {
                throw new Exception("Failed to update product stock.");
            }

            return inventory;
        }

        //! Update an inventory and modify product stock
        public async Task<Inventory> Update(string id, UpdateInventoryDto inventoryDto)
        {
            var existingInventory = await _inventoryModel.Find(i => i.Id == id).FirstOrDefaultAsync();
            if (existingInventory == null)
            {
                throw new Exception("Inventory not found.");
            }

            var stockDifference = inventoryDto.QuantityAvailable - existingInventory.QuantityAvailable;

            var filter = Builders<Inventory>.Filter.Eq(i => i.Id, id);
            var update = Builders<Inventory>.Update
                .Set(i => i.QuantityAvailable, inventoryDto.QuantityAvailable)
                .Set(i => i.UpdatedAt, DateTime.UtcNow);

            await _inventoryModel.UpdateOneAsync(filter, update);

            var updateSuccess = await UpdateProductStock(existingInventory.ProductId, stockDifference);
            if (!updateSuccess)
            {
                throw new Exception("Failed to update product stock.");
            }

            return existingInventory;
        }


//! Get inventories by vendor ID
public async Task<IEnumerable<Inventory>> GetByVendorId(string vendorId)
{
    return await _inventoryModel.Find(i => i.VendorId == vendorId).ToListAsync();
}


        //! Delete inventory and update product stock
        public async Task<Inventory> Delete(string id)
        {
            var inventory = await _inventoryModel.FindOneAndDeleteAsync(i => i.Id == id);
            if (inventory == null)
            {
                throw new Exception("Inventory not found.");
            }

            var updateSuccess = await UpdateProductStock(inventory.ProductId, -inventory.QuantityAvailable);
            if (!updateSuccess)
            {
                throw new Exception("Failed to update product stock.");
            }

            return inventory;
        }

        //! Get inventories by product ID
        public async Task<IEnumerable<Inventory>> GetByProductId(string productId)
        {
            return await _inventoryModel.Find(i => i.ProductId == productId).ToListAsync();
        }

        //! Helper method: Update product stock count
        private async Task<bool> UpdateProductStock(string productId, int quantityChange)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, productId);
            var update = Builders<Product>.Update.Inc(p => p.StockCount, quantityChange);

            var result = await _productModel.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}
