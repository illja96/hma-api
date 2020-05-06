using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.AutoMapper.Tests.Factory;
using HMA.BLL.Tier2.Services.Interfaces;
using HMA.BLL.Tier3.Services;
using HMA.DTO.Models.Tests.Stubs;
using HMA.DTO.Models.Transactions;
using HMA.DTO.ViewModels.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Moq;

namespace HMA.BLL.Tier3.Tests.Services
{
    [TestClass]
    public class TransactionT3ServiceTests
    {
        private readonly TransactionT3Service _transactionT3Service;

        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<ITransactionT2Service> _transactionT2Service;

        public TransactionT3ServiceTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _transactionT2Service = new Mock<ITransactionT2Service>();

            var mapper = AutoMapperFactory.Create();

            _transactionT3Service = new TransactionT3Service(
                mapper,
                _httpContextAccessorMock.Object,
                _transactionT2Service.Object);
        }

        [TestInitialize]
        public void Setup()
        {
            _httpContextAccessorMock.Reset();
            _transactionT2Service.Reset();
        }

        [TestMethod]
        public async Task GetUsedTagsByHouseIdAsync_ReturnsOkObjectResult()
        {
            // Arrange
            _httpContextAccessorMock
                .Setup(hca => hca.HttpContext)
                .Returns(HttpContextStubs.HttpContextWithUser);

            var tags = new List<string>();

            _transactionT2Service
                .Setup(tt2s => tt2s.GetUsedTagsByHouseIdAsync(
                    It.IsAny<BsonObjectId>(),
                    It.IsAny<decimal>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(tags);

            const string houseId = "000000000000000000000000";

            // Act
            var result = await _transactionT3Service.GetUsedTagsByHouseIdAsync(houseId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(result.Value, typeof(List<string>));
        }

        [TestMethod]
        public async Task GetTransactionsByHouseIdAsync_ReturnsOkObjectResult()
        {
            // Arrange
            _httpContextAccessorMock
                .Setup(hca => hca.HttpContext)
                .Returns(HttpContextStubs.HttpContextWithUser);

            var transactionInfos = new List<TransactionInfo>();

            _transactionT2Service
                .Setup(tt2s => tt2s.GetTransactionsByHouseIdAsync(
                    It.IsAny<BsonObjectId>(),
                    It.IsAny<decimal>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(transactionInfos);

            const string houseId = "000000000000000000000000";

            // Act
            var result = await _transactionT3Service.GetTransactionsByHouseIdAsync(houseId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(result.Value, typeof(List<TransactionInfoViewModel>));
        }

        [TestMethod]
        public async Task CreateTransactionAsync_ReturnsOkObjectResult()
        {
            // Arrange
            _httpContextAccessorMock
                .Setup(hca => hca.HttpContext)
                .Returns(HttpContextStubs.HttpContextWithUser);

            var transactionInfo = new TransactionInfo();

            _transactionT2Service
                .Setup(tt2s => tt2s.CreateTransactionAsync(
                    It.IsAny<TransactionCreationRequest>(),
                    It.IsAny<decimal>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(transactionInfo);

            var transactionCreationRequestViewModel = new TransactionCreationRequestViewModel()
            {
                HouseId = "000000000000000000000000"
            };

            // Act
            var result = await _transactionT3Service.CreateTransactionAsync(transactionCreationRequestViewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(result.Value, typeof(TransactionInfoViewModel));
        }

        [TestMethod]
        public async Task DeleteTransactionAsync_ReturnsOkObjectResult()
        {
            // Arrange
            _httpContextAccessorMock
                .Setup(hca => hca.HttpContext)
                .Returns(HttpContextStubs.HttpContextWithUser);

            const string houseId = "000000000000000000000000";
            const string transactionId = "000000000000000000000000";

            // Act
            var result = await _transactionT3Service.DeleteTransactionAsync(houseId, transactionId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNull(result.Value);
        }
    }
}
