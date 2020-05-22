using System.Collections.Generic;
using System.Security.Claims;
using HMA.BLL.Tier3.Services.Interfaces;
using HMA.DTO.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.BLL.Tier2.Services.Interfaces;
using HMA.DTO.Models.Transactions;
using HMA.DTO.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;

namespace HMA.BLL.Tier3.Services
{
    public class TransactionT3Service : ITransactionT3Service
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ITransactionT2Service _transactionT2Service;

        public TransactionT3Service(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ITransactionT2Service transactionT2Service)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

            _transactionT2Service = transactionT2Service;
        }

        public async Task<ObjectResult> GetUsedTagsByHouseIdAsync(
            string houseId,
            CancellationToken cancellationToken = default)
        {
            BsonObjectId bsonHouseId;
            try
            {
                bsonHouseId = _mapper.Map<BsonObjectId>(houseId);
            }
            catch (AutoMapperMappingException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(houseId), "Invalid format");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            try
            {
                var userFromIdentity = GetUserFromIdentity();

                var tags = await _transactionT2Service.GetUsedTagsByHouseIdAsync(
                    bsonHouseId,
                    userFromIdentity.GoogleId,
                    cancellationToken);

                var result = new OkObjectResult(tags);
                return result;
            }
            catch (HouseNotFoundException)
            {
                var badResult = new NotFoundObjectResult(null);
                return badResult;
            }
        }

        public async Task<ObjectResult> GetTransactionsByHouseIdAsync(
            string houseId,
            CancellationToken cancellationToken = default)
        {
            BsonObjectId bsonHouseId;
            try
            {
                bsonHouseId = _mapper.Map<BsonObjectId>(houseId);
            }
            catch (AutoMapperMappingException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(houseId), "Invalid format");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            try
            {
                var userFromIdentity = GetUserFromIdentity();

                var transactionInfos = await _transactionT2Service.GetTransactionsByHouseIdAsync(
                    bsonHouseId,
                    userFromIdentity.GoogleId,
                    cancellationToken);

                var transactionInfoViewModels = _mapper.Map<List<TransactionInfoViewModel>>(transactionInfos);

                var result = new OkObjectResult(transactionInfoViewModels);
                return result;
            }
            catch (HouseNotFoundException)
            {
                var badResult = new NotFoundObjectResult(null);
                return badResult;
            }
        }

        public async Task<ObjectResult> CreateTransactionAsync(
            TransactionCreationRequestViewModel transactionCreationRequestViewModel,
            CancellationToken cancellationToken = default)
        {
            TransactionCreationRequest transactionCreationRequest;
            try
            {
                transactionCreationRequest = _mapper.Map<TransactionCreationRequest>(transactionCreationRequestViewModel);
            }
            catch (AutoMapperMappingException e)
            {
                if (!string.Equals(e.MemberMap.DestinationName, nameof(TransactionCreationRequest.HouseId)))
                {
                    throw;
                }
                
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(TransactionCreationRequestViewModel.HouseId), "Invalid format");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            var userFromIdentity = GetUserFromIdentity();
            transactionCreationRequest.CreatedBy = userFromIdentity.GoogleId;

            var transactionInfo = await _transactionT2Service.CreateTransactionAsync(
                transactionCreationRequest,
                userFromIdentity.GoogleId,
                cancellationToken);

            var transactionInfoViewModel = _mapper.Map<TransactionInfoViewModel>(transactionInfo);

            var result = new OkObjectResult(transactionInfoViewModel);
            return result;
        }

        public async Task<ObjectResult> DeleteTransactionAsync(
            string houseId,
            string transactionId,
            CancellationToken cancellationToken = default)
        {
            BsonObjectId bsonHouseId;
            try
            {
                bsonHouseId = _mapper.Map<BsonObjectId>(houseId);
            }
            catch (AutoMapperMappingException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(houseId), "Invalid format");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            BsonObjectId bsonTransactionId;
            try
            {
                bsonTransactionId = _mapper.Map<BsonObjectId>(transactionId);
            }
            catch (AutoMapperMappingException)
            {
                var modelState = new ModelStateDictionary();
                modelState.AddModelError(nameof(transactionId), "Invalid format");

                var badResult = new BadRequestObjectResult(modelState);
                return badResult;
            }

            var userFromIdentity = GetUserFromIdentity();

            await _transactionT2Service.DeleteTransactionAsync(
                bsonHouseId,
                bsonTransactionId,
                userFromIdentity.GoogleId,
                cancellationToken);

            var result = new OkObjectResult(string.Empty);
            return result;
        }

        private UserInfo GetUserFromIdentity()
        {
            var claimsIdentity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userFromIdentity = _mapper.Map<UserInfo>(claimsIdentity);

            return userFromIdentity;
        }
    }
}
