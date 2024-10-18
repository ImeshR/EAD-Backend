/*
 * File: Order Tracking Service
 * Author: Fernando B.K.M.
 * Description: This file contains the business logic for Order Tracking management.
*/
using EAD_Backend.Data;
using EAD_Backend.DTOs;
using EAD_Backend.Models;
using MongoDB.Driver;

namespace EAD_Backend.Services
{
    public class OrderTrackingService
    {
        private readonly IConfiguration _configuration;

        private readonly IMongoCollection<OrderTracking> _orderTrackingModel;

        public OrderTrackingService(MongoDBService mongoDbService, IConfiguration configuration)
        {
            _configuration = configuration;
            _orderTrackingModel = mongoDbService.Database?.GetCollection<OrderTracking>("orderTrackings");
        }

        public async Task<IEnumerable<OrderTracking>> GetAll()
        {
            return await _orderTrackingModel.Find(product => true).ToListAsync();
        }

        public async Task<OrderTracking> Create(string orderId, OrderTracking orderTracking)
        {
            orderTracking.OrderId = orderId;
            await _orderTrackingModel.InsertOneAsync(orderTracking);
            return orderTracking;
        }

        public async Task<OrderTracking> Delete(string id)
        {
            var filter = Builders<OrderTracking>.Filter.Eq("Id", id);
            return await _orderTrackingModel.FindOneAndDeleteAsync(filter);
        }

    }
}