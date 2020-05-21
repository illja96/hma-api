using System.Collections.Generic;
using HMA.BLL.Tier3.Services.Interfaces;
using HMA.DTO.ViewModels.Transactions;
using HMA.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace HMA.API.Controllers
{
    [ApiController]
    [Authorize(PolicyConstants.UserRegistered)]
    public class TransactionController : Controller
    {
        private readonly ITransactionT3Service _transactionT3Service;

        public TransactionController(ITransactionT3Service transactionT3Service)
        {
            _transactionT3Service = transactionT3Service;
        }

        /// <summary>
        /// Get already used transaction tags by house id
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpGet("house/{houseId}/transactions/tags")]
        [ProducesResponseType(typeof(List<string>), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUsedTransactionTagsByHouseIdAsync(
            [FromRoute] string houseId,
            CancellationToken cancellationToken = default)
        {
            var result = await _transactionT3Service.GetUsedTagsByHouseIdAsync(houseId, cancellationToken);
            return result;
        }

        /// <summary>
        /// Get transactions by house id
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpGet("houses/{houseId}/transactions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetTransactionsByHouseIdAsync(
            [FromRoute] string houseId,
            CancellationToken cancellationToken = default)
        {
            var result = await _transactionT3Service.GetTransactionsByHouseIdAsync(houseId, cancellationToken);
            return result;
        }

        /// <summary>
        /// Create transaction
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="transactionCreationRequestViewModel">Transaction creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpPost("houses/{houseId}/transactions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateTransactionAsync(
            [FromRoute] string houseId,
            [FromBody] TransactionCreationRequestViewModel transactionCreationRequestViewModel,
            CancellationToken cancellationToken = default)
        {
            if (!string.Equals(houseId, transactionCreationRequestViewModel.HouseId))
            {
                ModelState.AddModelError(nameof(transactionCreationRequestViewModel.HouseId), "Missmatching house id in request url and body");
            }

            if (!ModelState.IsValid)
            {
                var badResult = new BadRequestObjectResult(ModelState);
                return badResult;
            }

            var result = await _transactionT3Service.CreateTransactionAsync(transactionCreationRequestViewModel, cancellationToken);
            return result;
        }


        /// <summary>
        /// Delete transaction
        /// </summary>
        /// <param name="houseId">House id</param>
        /// <param name="transactionId">Transaction id</param>
        /// <param name="cancellationToken">Cancellation token</param>
        [HttpDelete("houses/{houseId}/transactions/{transactionId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteTransactionAsync(
            [FromRoute] string houseId,
            [FromRoute] string transactionId,
            CancellationToken cancellationToken = default)
        {
            var result = await _transactionT3Service.DeleteTransactionAsync(houseId, transactionId, cancellationToken);
            return result;
        }
    }
}