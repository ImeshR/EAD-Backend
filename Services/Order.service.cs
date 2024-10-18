/*
 * File: Order Service
 * Author: Fernando B.K.M.
 * Description: This file contains the business logic for Order management.
*/

using EAD_Backend.Data;
using EAD_Backend.DTOs;
using EAD_Backend.Models;
using MongoDB.Driver;

namespace EAD_Backend.Services
{
    public class OrderServices
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoCollection<Order> _orderModel;
        private readonly NotificationService _notificationService;

        public OrderServices(MongoDBService mongoDbService, IConfiguration configuration, NotificationService notificationService)
        {
            _configuration = configuration;
            _orderModel = mongoDbService.Database?.GetCollection<Order>("orders");
            _notificationService = notificationService;
        }

        //! Create Order
        public async Task<Order> Create(string? userID, Order order)
        {
            order.CustomerId = userID;

            // Ensure default status is set to "Pending"
            order.Status = string.IsNullOrEmpty(order.Status) ? "Pending" : order.Status;

            order.CreatedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;

            // Insert order into the database
            await _orderModel.InsertOneAsync(order);

            // Create a notification for the customer
            var notification = new Notification
            {
                UserId = userID!,
                Message = $"Your order with ID {order.Id} has been placed successfully.",
                Type = "Order",
                ReadStatus = false,
                CreatedAt = DateTime.UtcNow
            };

            // Send the notification
            await _notificationService.Create(userID!, notification);

            return order;
        }

        //! Get All Orders
        public async Task<IEnumerable<Order>> GetAll()
        {
            return await _orderModel.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetPendingOrders()
        {
            var filter = Builders<Order>.Filter.Eq(order => order.Status, "Pending");
            return await _orderModel.Find(filter).ToListAsync();
        }


        //! Update an Order
        public async Task<object> Update(string id, UpdateOrderDto order)
        {
            var filter = Builders<Order>.Filter.Eq("Id", id);
            var update = Builders<Order>.Update
                .Set("totalAmount", order.TotalAmount)
                .Set("status", order.Status)
                .Set("createdAt", order.CreatedAt)
                .Set("updatedAt", order.UpdatedAt);

            await _orderModel.UpdateOneAsync(filter, update);
            return order;
        }

        //! Delete an Order and notify admin
        public async Task<Order?> Delete(string id)
        {
            var filter = Builders<Order>.Filter.Eq("Id", id);

            // Delete the order from the database
            var deletedOrder = await _orderModel.FindOneAndDeleteAsync(filter);

            if (deletedOrder != null)
            {
                // Notify admin about the deleted order
                var adminId = _configuration["Admin:Id"]; // Ensure this is set in appsettings.json

                var notification = new Notification
                {
                    UserId = adminId,
                    Message = $"Order with ID {deletedOrder.Id} has been deleted.",
                    Type = "OrderDeletion",
                    ReadStatus = false,
                    CreatedAt = DateTime.UtcNow
                };

                await _notificationService.Create(adminId, notification);
            }

            return deletedOrder;
        }




        //! Update Order Status
        public async Task<Order?> UpdateStatus(string orderId, string newStatus)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, orderId);
            var update = Builders<Order>.Update.Set(o => o.Status, newStatus);

            var updatedOrder = await _orderModel.FindOneAndUpdateAsync(filter, update);

            return updatedOrder;
        }




        //! Get order items by vendor ID
        public async Task<IEnumerable<OrderItemDto>> GetOrderItemsByVendorId(string vendorId)
        {
            var orders = await _orderModel.Find(_ => true).ToListAsync();
            var orderItemsDto = new List<OrderItemDto>();

            foreach (var order in orders)
            {
                var itemsForVendor = order.OrderItems.Where(item => item.VendorId == vendorId);

                foreach (var item in itemsForVendor)
                {
                    orderItemsDto.Add(new OrderItemDto
                    {
                        OrderId = order.Id,
                        CustomerId = order.CustomerId,
                        OrderStatus = order.Status,
                        Item = item
                    });
                }
            }

            return orderItemsDto;
        }
 // Delete orders by customer ID
        public async Task<bool> DeleteByCustomerId(string customerId)
        {
            var filter = Builders<Order>.Filter.Eq(order => order.CustomerId, customerId);
            var result = await _orderModel.DeleteManyAsync(filter);

            // Return true if any orders were deleted
            return result.DeletedCount > 0;
        }


        // Get orders by Customer ID
        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(string customerId)
        {
            var filter = Builders<Order>.Filter.Eq(order => order.CustomerId, customerId);
            return await _orderModel.Find(filter).ToListAsync();
        }

    }
}
