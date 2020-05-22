using System.Threading;
using System.Threading.Tasks;
using HMA.AutoMapper.Tests.Factory;
using HMA.BLL.Tier2.Services.Interfaces;
using HMA.BLL.Tier3.Services;
using HMA.DTO.Models.House;
using HMA.DTO.Models.Tests.Stubs;
using HMA.DTO.ViewModels.House;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Moq;

namespace HMA.BLL.Tier3.Tests.Services
{
    [TestClass]
    public class HouseT3ServiceTests
    {
        private readonly HouseT3Service _houseT3Service;

        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IHouseT2Service> _houseT2ServiceMock;

        public HouseT3ServiceTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _houseT2ServiceMock = new Mock<IHouseT2Service>();

            var mapper = AutoMapperFactory.Create();

            _houseT3Service = new HouseT3Service(
                mapper,
                _httpContextAccessorMock.Object,
                _houseT2ServiceMock.Object);
        }

        [TestInitialize]
        public void Setup()
        {
            _httpContextAccessorMock.Reset();
            _houseT2ServiceMock.Reset();
        }

        [TestMethod]
        public async Task GetAvailableHouseInfosForUserAsync_ReturnsOkObjectResult()
        {
            // Arrange
            _httpContextAccessorMock
                .Setup(hca => hca.HttpContext)
                .Returns(HttpContextStubs.HttpContextWithUser);

            var availableHousesInfo = new AvailableHousesInfo();

            _houseT2ServiceMock
                .Setup(ht2s => ht2s.GetAvailableHouseInfosForUserAsync(
                    It.IsAny<decimal>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(availableHousesInfo);

            // Act
            var result = await _houseT3Service.GetHousesAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(result.Value, typeof(AvailableHousesInfoViewModel));
        }

        [TestMethod]
        public async Task GetHouseSimpleInfoByIdAvailableForUserAsync_ReturnsOkObjectResult()
        {
            // Arrange
            _httpContextAccessorMock
                .Setup(hca => hca.HttpContext)
                .Returns(HttpContextStubs.HttpContextWithUser);

            const string houseId = "000000000000000000000000";

            var houseSimpleInfo = new HouseSimpleInfo()
            {
                Id = ObjectId.Parse(houseId)
            };

            _houseT2ServiceMock
                .Setup(ht2s => ht2s.GetHouseSimpleInfoByIdAvailableForUserAsync(
                    It.IsAny<BsonObjectId>(),
                    It.IsAny<decimal>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(houseSimpleInfo);

            // Act
            var result = await _houseT3Service.GetHouseByIdAsync(houseId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(result.Value, typeof(HouseSimpleInfoViewModel));
        }

        [TestMethod]
        public async Task CreateHouseAsync_ReturnsOkObjectResult()
        {
            // Arrange
            _httpContextAccessorMock
                .Setup(hca => hca.HttpContext)
                .Returns(HttpContextStubs.HttpContextWithUser);

            var houseSimpleInfo = new HouseSimpleInfo();

            _houseT2ServiceMock
                .Setup(ht2s => ht2s.CreateHouseAsync(
                    It.IsAny<HouseCreationRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(houseSimpleInfo);

            var houseCreationRequestViewModel = new HouseCreationRequestViewModel();

            // Act
            var result = await _houseT3Service.CreateHouseAsync(houseCreationRequestViewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(result.Value, typeof(HouseSimpleInfoViewModel));
        }

        // TODO: Update tests after implementing DeleteOrLeaveHouseByIdAsync
        [TestMethod]
        public async Task DeleteOrLeaveHouseByIdAsyncByIdAsync_ReturnsOkObjectResult()
        {
            // Arrange
            _httpContextAccessorMock
                .Setup(hca => hca.HttpContext)
                .Returns(HttpContextStubs.HttpContextWithUser);

            const string houseId = "000000000000000000000000";
            
            // Act
            var result = await _houseT3Service.DeleteOrLeaveHouseByIdAsync(houseId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNull(result.Value, null);
        }
    }
}
