using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Services.Interfaces;
using HMA.BLL.Tier2.Services.Interfaces;
using HMA.DTO.Models.Transactions;
using MongoDB.Bson;

namespace HMA.BLL.Tier2.Services
{
    public class TransactionT2Service : ITransactionT2Service
    {
        private readonly ITransactionT1Service _transactionT1Service;

        public TransactionT2Service(ITransactionT1Service transactionT1Service)
        {
            _transactionT1Service = transactionT1Service;
        }

        public async Task<List<string>> GetUsedTagsByHouseIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var tags = await _transactionT1Service.GetUsedTagsByHouseIdAsync(
                houseId,
                userId, 
                cancellationToken);

            return tags;
        }

        public async Task<List<TransactionInfo>> GetTransactionsByHouseIdAsync(
            BsonObjectId houseId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var transactions = await _transactionT1Service.GetTransactionsByHouseIdAsync(
                houseId,
                userId,
                cancellationToken);

            return transactions;
        }

        public async Task<TransactionInfo> CreateTransactionAsync(
            TransactionCreationRequest transactionCreationRequest,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            var transaction = await _transactionT1Service.CreateTransactionAsync(
                transactionCreationRequest,
                userId,
                cancellationToken);

            return transaction;
        }

        public async Task DeleteTransactionAsync(
            BsonObjectId houseId,
            BsonObjectId transactionId,
            decimal userId,
            CancellationToken cancellationToken = default)
        {
            await _transactionT1Service.DeleteTransactionAsync(
                houseId,
                transactionId,
                userId,
                cancellationToken);
        }
    }
}
