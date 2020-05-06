using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.BLL.Tier1.Exceptions.Transaction;
using HMA.DTO.Models.Transactions;
using MongoDB.Bson;

namespace HMA.BLL.Tier1.Services.Interfaces
{
    /// <summary>
    /// Transaction service
    /// </summary>
    public interface ITransactionT1Service
    {
        /// <summary>
        /// Get already used transaction tags by house id for provided user
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="HouseNotFoundException"></exception>
        Task<List<string>> GetUsedTagsByHouseIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get transactions by house id for provided user
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task<List<TransactionInfo>> GetTransactionsByHouseIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create transaction for provided user
        /// </summary>
        /// <param name="transactionCreationRequest">Transaction creation request</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public Task<TransactionInfo> CreateTransactionAsync(
            TransactionCreationRequest transactionCreationRequest,
            decimal userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete transaction for provided user
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="transactionId">Transaction id</param>
        /// <param name="userId">User id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="TransactionNotFoundException"></exception>
        public Task DeleteTransactionAsync(
            BsonObjectId houseId,
            BsonObjectId transactionId,
            decimal userId,
            CancellationToken cancellationToken = default);
    }
}
