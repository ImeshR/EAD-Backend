/*
 * File: Category Service
 * Author: Fernando B.K.M.
 * Description: This file contains the business logic for Category management.
*/

using EAD_Backend.Data;
using EAD_Backend.DTOs;
using EAD_Backend.Models;
using MongoDB.Driver;

namespace EAD_Backend.Services
{
    public class CategoryService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<Category> _categoryModel;

        public CategoryService(MongoDBService mongoDbService, IConfiguration configuration)
        {
            _configuration = configuration;
            _categoryModel = mongoDbService.Database?.GetCollection<Category>("categories");
        }


        //! Get all categories
        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryModel.Find(category => true).ToListAsync();
        }

//! Get category by ID
        public async Task<Category> GetById(string id)
        {
            var filter = Builders<Category>.Filter.Eq("Id", id);
            return await _categoryModel.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Category> Create(string productId, Category category)
        {

            //category.Id = productId;
            await _categoryModel.InsertOneAsync(category);
            return category;
        }





        public async Task<object> Update(string id, UpdateCategoryDto categoryDto)

        {
            var filter = Builders<Category>.Filter.Eq("Id", id);
            var update = Builders<Category>.Update
                .Set("categoryId", categoryDto.CategoryId)
                .Set("name", categoryDto.Name)
                .Set("description", categoryDto.Description)
                .Set("status", categoryDto.Status);

            await _categoryModel.UpdateOneAsync(filter, update);
            return categoryDto;
        }


        //! Delete 
        public async Task<Category> Delete(string id)
        {
            var filter = Builders<Category>.Filter.Eq("Id", id);
            return await _categoryModel.FindOneAndDeleteAsync(filter);
        }

        // Retrieve categories with status = "true"
        public async Task<IEnumerable<Category>> GetActiveCategories()
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Status, "true");
            return await _categoryModel.Find(filter).ToListAsync();
        }






    }

}