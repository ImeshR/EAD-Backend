/*
 * File: Review Service
 * Author: Fernando B.K.M.
 * Description: This file contains the business logic for Review management.
 */

using EAD_Backend.Data;
using EAD_Backend.DTOs;
using EAD_Backend.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAD_Backend.Services
{
    public class ReviewService
    {
        private readonly IMongoCollection<Review> _reviewModel;

        public ReviewService(MongoDBService mongoDbService)
        {
            _reviewModel = mongoDbService.Database?.GetCollection<Review>("reviews");
        }

        // Get all reviews
        public async Task<IEnumerable<Review>> GetAll()
        {
            return await _reviewModel.Find(_ => true).ToListAsync();
        }

        // Create a new review
        public async Task<Review> Create(Review review)
        {
            await _reviewModel.InsertOneAsync(review);
            return review;
        }

        // Create a review with user ID
        public async Task<Review> Create(string? userId, Review review)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty.");
            }

            review.CustomerId = userId;
            await _reviewModel.InsertOneAsync(review);
            return review;
        }

        // Get reviews by Vendor ID
        public async Task<IEnumerable<Review>> GetByVendorId(string vendorId)
        {
            if (string.IsNullOrEmpty(vendorId))
            {
                throw new ArgumentException("Vendor ID cannot be null or empty.");
            }

            var filter = Builders<Review>.Filter.Eq(r => r.VendorId, vendorId);
            return await _reviewModel.Find(filter).ToListAsync();
        }

        // Delete a review by ID
        public async Task<Review> Delete(string id)
        {
            var filter = Builders<Review>.Filter.Eq(r => r.Id, id);
            return await _reviewModel.FindOneAndDeleteAsync(filter);
        }

        // Add a reply to a review
        public async Task<Review> AddReply(string reviewId, Reply reply)
        {
            var filter = Builders<Review>.Filter.Eq(r => r.Id, reviewId);
            var update = Builders<Review>.Update
                .Set(r => r.UpdatedAt, DateTime.UtcNow) // Update timestamp
                .Push(r => r.Replies, reply); // Add the reply

            var updatedReview = await _reviewModel.FindOneAndUpdateAsync(
                filter,
                update,
                new FindOneAndUpdateOptions<Review>
                {
                    ReturnDocument = ReturnDocument.After // Return updated review
                });

            if (updatedReview == null)
            {
                throw new Exception("Review not found.");
            }

            return updatedReview;
        }

        // Get replies for a specific review
        public async Task<List<Reply>> GetReplies(string reviewId)
        {
            var review = await _reviewModel.Find(r => r.Id == reviewId).FirstOrDefaultAsync();
            if (review == null)
            {
                throw new Exception("Review not found.");
            }

            return review.Replies;
        }
    }
}
