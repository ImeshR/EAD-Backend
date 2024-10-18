/*
 * File: Product Service
 * Author: Fernando B.K.M.
 * Description: This file contains the business logic for Product management.
*/
using EAD_Backend.Data;
using EAD_Backend.DTOs;
using EAD_Backend.Models;
using MongoDB.Driver;

namespace EAD_Backend.Services
{
    public class ProductService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<Product> _productModel;

        public ProductService(MongoDBService mongoDbService, IConfiguration configuration)
        {
            _configuration = configuration;
            _productModel = mongoDbService.Database?.GetCollection<Product>("products");
        }

        //! =======================================================  Define Business | DB Operations for Product ===================================>
        //! Get all products
        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _productModel.Find(product => true).ToListAsync();
        }

        //! Get product by ID
        public async Task<Product> GetById(string id)
        {
            var filter = Builders<Product>.Filter.Eq("Id", id);
            return await _productModel.Find(filter).FirstOrDefaultAsync();
        }


        //! Get products by category ID
        public async Task<IEnumerable<Product>> GetByCategoryId(string categoryId)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.CategoryId, categoryId);
            return await _productModel.Find(filter).ToListAsync();
        }



        //! create a product
        public async Task<Product> Create(Product product)
        {

            //  product.VendorId = userId;
            await _productModel.InsertOneAsync(product);
            return product;
        }

        //! Get products by vendor ID
        public async Task<IEnumerable<Product>> GetByVendorId(string vendorId)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.VendorId, vendorId);
            return await _productModel.Find(filter).ToListAsync();
        }

        //! Update a product
        public async Task<object> Update(string id, UpdateProductDto product)
        {
            var filter = Builders<Product>.Filter.Eq("Id", id);
            var update = Builders<Product>.Update
                .Set("Name", product.Name)
                .Set("Description", product.Description)
                .Set("Price", product.Price)
                .Set("CategoryId", product.CategoryId)
                .Set("Images", product.Images)
                .Set("Active", product.Active)
                .Set("StockCount", product.StockCount);

            await _productModel.UpdateOneAsync(filter, update);
            return product;
        }

        //! Delete a product
        public async Task<Product> Delete(string id)
        {
            var filter = Builders<Product>.Filter.Eq("Id", id);
            return await _productModel.FindOneAndDeleteAsync(filter);
        }

    }
}

