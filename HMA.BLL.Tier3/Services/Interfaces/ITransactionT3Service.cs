using HMA.DTO.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace HMA.BLL.Tier3.Services.Interfaces
{
    /// <summary>
    /// Transaction service
    /// </summary>
    public interface ITransactionT3Service
    {
        /// <summary>
        /// Get already used transaction tags by house id
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> GetUsedTagsByHouseIdAsync(
            string houseId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get transactions by house id
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> GetTransactionsByHouseIdAsync(
            string houseId, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Create transaction
        /// </summary>
        /// <param name="transactionCreationRequestViewModel">Transaction creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> CreateTransactionAsync(
            TransactionCreationRequestViewModel transactionCreationRequestViewModel,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete transaction
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="transactionId">Transaction id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<ObjectResult> DeleteTransactionAsync(
            string houseId,
            string transactionId,
            CancellationToken cancellationToken = default);
    }
}
