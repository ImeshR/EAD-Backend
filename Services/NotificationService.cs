/*
 * File: Notification Service
 * Author: Fernando B.K.M.
 * Description: This file contains the business logic for Notification management.
*/

using EAD_Backend.Data;
using EAD_Backend.DTOs;
using EAD_Backend.Models;
using MongoDB.Driver;

namespace EAD_Backend.Services
{
    public class NotificationService
    {

        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<Notification> _notificationModel;

        public NotificationService(MongoDBService mongoDbService, IConfiguration configuration)
        {
            _configuration = configuration;
            _notificationModel = mongoDbService.Database?.GetCollection<Notification>("notifications");
        }


        //Get all notifications
        public async Task<IEnumerable<Notification>> GetAll()
        {
            return await _notificationModel.Find(product => true).ToListAsync();
        }


        //! Retrieve notifications by type
        public async Task<IEnumerable<Notification>> GetByType(string type)
        {
            var filter = Builders<Notification>.Filter.Eq("Type", type);
            return await _notificationModel.Find(filter).ToListAsync();
        }


        //! create 
        public async Task<Notification> Create(string userId, Notification notification)
        {

            notification.UserId = userId;
            await _notificationModel.InsertOneAsync(notification);
            return notification;
        }

        public async Task<object> Update(string id, UpdateNotificationDto notificationDto)
        {
            var filter = Builders<Notification>.Filter.Eq("Id", id);
            var update = Builders<Notification>.Update
                .Set("userId", notificationDto.UserId)
                .Set("message", notificationDto.Message)
                .Set("type", notificationDto.Type)
                .Set("readStatus", notificationDto.ReadStatus)
                .Set("createdAt", notificationDto.CreatedAt);

            await _notificationModel.UpdateOneAsync(filter, update);
            return notificationDto;
        }

        //! Delete 
        public async Task<Notification> Delete(string id)
        {
            var filter = Builders<Notification>.Filter.Eq("Id", id);
            return await _notificationModel.FindOneAndDeleteAsync(filter);
        }


// Get notifications by User ID
public async Task<IEnumerable<Notification>> GetByUserId(string userId)
{
    var filter = Builders<Notification>.Filter.Eq(notification => notification.UserId, userId);
    return await _notificationModel.Find(filter).ToListAsync();
}





        // Delete all notifications by user ID
        public async Task<bool> DeleteByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.");

            var filter = Builders<Notification>.Filter.Eq(notification => notification.UserId, userId);
            var result = await _notificationModel.DeleteManyAsync(filter);

            // Return true if any notifications were deleted
            return result.DeletedCount > 0;
        }


        public async Task<Notification> DeleteByIdAndUserId(string notificationId, string userId)
{
    if (string.IsNullOrWhiteSpace(notificationId) || string.IsNullOrWhiteSpace(userId))
        throw new ArgumentException("Notification ID and User ID cannot be null or empty.");

    var filter = Builders<Notification>.Filter.And(
        Builders<Notification>.Filter.Eq(notification => notification.Id, notificationId),
        Builders<Notification>.Filter.Eq(notification => notification.UserId, userId)
    );

    return await _notificationModel.FindOneAndDeleteAsync(filter);
}


    }


}
