/*
 * File: Payment Service
 * Author: Fernando B.K.M.
 * Description: This file contains the business logic for Payment management.
 */

using EAD_Backend.Data;
using EAD_Backend.Models;
using MongoDB.Driver;

namespace EAD_Backend.Services
{
    public class PaymentService
    {
        private readonly IMongoCollection<Payment> _paymentModel;

        public PaymentService(MongoDBService mongoDbService)
        {
            _paymentModel = mongoDbService.Database?.GetCollection<Payment>("payments");
        }

        //! =======================================================  Define Business | DB Operations for Payment ====================================>

        //! Get all payments
        public async Task<IEnumerable<Payment>> GetAll()
        {
            return await _paymentModel.Find(payment => true).ToListAsync();
        }

        //! Get payment by ID
        public async Task<Payment?> GetById(string id)
        {
            return await _paymentModel.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        //! Create a payment
        /* public async Task<Payment> Create(Payment payment)
        {
            await _paymentModel.InsertOneAsync(payment);
            return payment;
        } */






        //! Create a payment and deduct 1 from product stock
        public async Task<Payment> Create(Payment payment)
        {
            // Start a transaction to ensure data consistency
            using var session = await _paymentModel.Database.Client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                // Insert the payment within the session
                await _paymentModel.InsertOneAsync(session, payment);

                // Access the product collection
                var productCollection = _paymentModel.Database.GetCollection<Product>("products");

                // Define the filter and update to deduct 1 from the product stock
                var filter = Builders<Product>.Filter.Eq(p => p.Id, payment.ProductId);
                var update = Builders<Product>.Update.Inc(p => p.StockCount, -1);

                // Perform the stock update within the session
                var updatedProduct = await productCollection.FindOneAndUpdateAsync(
                    session,
                    filter,
                    update,
                    new FindOneAndUpdateOptions<Product, Product>
                    {
                        ReturnDocument = ReturnDocument.After // Return the updated product after modification
                    }
                );

                // Commit the transaction if both operations succeed
                await session.CommitTransactionAsync();

                return payment;
            }
            catch (Exception)
            {
                // Abort the transaction on error to ensure consistency
                await session.AbortTransactionAsync();
                throw;
            }
        }


        //! Update payment status
        public async Task<Payment?> UpdatePaymentStatus(string id, string newStatus)
        {
            var filter = Builders<Payment>.Filter.Eq("Id", id);
            var update = Builders<Payment>.Update
                .Set("paymentStatus", newStatus)
                .Set("updatedAt", DateTime.UtcNow);

            var result = await _paymentModel.FindOneAndUpdateAsync(filter, update);
            return result;
        }

        //! Delete a payment
        public async Task<Payment?> Delete(string id)
        {
            var filter = Builders<Payment>.Filter.Eq("Id", id);
            return await _paymentModel.FindOneAndDeleteAsync(filter);
        }
    }
}
