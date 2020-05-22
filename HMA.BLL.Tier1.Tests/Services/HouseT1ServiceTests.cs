using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HMA.AutoMapper.Tests.Factory;
using HMA.BLL.Tier1.Exceptions.House;
using HMA.BLL.Tier1.Services;
using HMA.DAL.Repositories.Interfaces;
using HMA.DTO.Models.House;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace HMA.BLL.Tier1.Tests.Services
{
    [TestClass]
    public class HouseT1ServiceTests
    {
        private readonly HouseT1Service _houseService;

        private readonly Mock<IGenericRepository<HouseInfo>> _houseRepositoryMock;

        public HouseT1ServiceTests()
        {
            var mapper = AutoMapperFactory.Create();

            _houseRepositoryMock = new Mock<IGenericRepository<HouseInfo>>();

            _houseService = new HouseT1Service(
                mapper,
                _houseRepositoryMock.Object);
        }

        [TestInitialize]
        public void Setup()
        {
            _houseRepositoryMock.Reset();
        }

        [TestMethod]
        public async Task CreateHouseAsync_SetsCreationDate()
        {
            // Arrange
            var houseCreationRequest = new HouseCreationRequest();

            _houseRepositoryMock
                .Setup(hr => hr.InsertAsync(
                    It.IsAny<HouseInfo>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((HouseInfo hi, CancellationToken ct) => hi);

            // Act
            var houseInfo = await _houseService.CreateHouseAsync(houseCreationRequest);

            // Assert
            var isCreationDateNearUtcNow = DateTime.UtcNow.AddMinutes(-1) < houseInfo.CreationDate &&
                                           houseInfo.CreationDate < DateTime.Now.AddMinutes(1);
            Assert.IsTrue(isCreationDateNearUtcNow);
        }

        [TestMethod]
        public async Task GetHouseInfoByIdAvailableForUserAsync_ReturnsHouseInfo()
        {
            // Arrange
            var houseInfo = new HouseInfo()
            {
                Id = ObjectId.GenerateNewId(),
                OwnerId = 123456789,
                MemberIds = new List<decimal>() { 123456789 }
            };

            _houseRepositoryMock
                .Setup(hr => hr.FindOneAsync(
                    It.IsAny<PipelineDefinition<HouseInfo, HouseInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(houseInfo);

            // Act
            var result = await _houseService.GetHouseInfoByIdAvailableForUserAsync(houseInfo.Id, houseInfo.OwnerId);

            // Assert
            Assert.AreSame(houseInfo, result);
        }

        [TestMethod]
        public async Task GetHouseInfoByIdAvailableForUserAsync_HouseNotFound_ThrowsHouseNotFoundException()
        {
            // Arrange
            var houseInfo = new HouseInfo()
            {
                Id = ObjectId.GenerateNewId(),
                OwnerId = 123456789,
                MemberIds = new List<decimal>() { 123456789 }
            };

            var exception = new InvalidOperationException("Sequence contains no elements");

            _houseRepositoryMock
                .Setup(hr => hr.FindOneAsync(
                    It.IsAny<PipelineDefinition<HouseInfo, HouseInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws(exception);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<HouseNotFoundException>(
                () => _houseService.GetHouseInfoByIdAvailableForUserAsync(houseInfo.Id, houseInfo.OwnerId));
        }

        [TestMethod]
        public async Task GetHouseInfoByIdAvailableForUserAsync_InvalidOperationException_ThrowsException()
        {
            // Arrange
            var houseInfo = new HouseInfo()
            {
                Id = ObjectId.GenerateNewId(),
                OwnerId = 123456789,
                MemberIds = new List<decimal>() { 123456789 }
            };

            _houseRepositoryMock
                .Setup(hr => hr.FindOneAsync(
                    It.IsAny<PipelineDefinition<HouseInfo, HouseInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _houseService.GetHouseInfoByIdAvailableForUserAsync(houseInfo.Id, houseInfo.OwnerId));
        }

        [TestMethod]
        public async Task GetHouseInfoByIdAvailableForUserAsync_TimeoutException_ThrowsException()
        {
            // Arrange
            var houseInfo = new HouseInfo()
            {
                Id = ObjectId.GenerateNewId(),
                OwnerId = 123456789,
                MemberIds = new List<decimal>() { 123456789 }
            };

            _houseRepositoryMock
                .Setup(hr => hr.FindOneAsync(
                    It.IsAny<PipelineDefinition<HouseInfo, HouseInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws<TimeoutException>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<TimeoutException>(
                () => _houseService.GetHouseInfoByIdAvailableForUserAsync(houseInfo.Id, houseInfo.OwnerId));
        }

        [TestMethod]
        public async Task GetSimpleHouseByIdAvailableForUserAsync_ReturnsHouseSimpleInfo()
        {
            // Arrange
            var houseSimpleInfo = new HouseSimpleInfo()
            {
                Id = ObjectId.GenerateNewId(),
                OwnerId = 123456789,
                MemberIds = new List<decimal>() { 123456789 }
            };

            _houseRepositoryMock
                .Setup(hr => hr.FindOneAsync(
                    It.IsAny<PipelineDefinition<HouseInfo, HouseSimpleInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(houseSimpleInfo);

            // Act
            var result = await _houseService.GetSimpleHouseInfoByIdAvailableForUserAsync(houseSimpleInfo.Id, houseSimpleInfo.OwnerId);

            // Assert
            Assert.AreSame(houseSimpleInfo, result);
        }

        [TestMethod]
        public async Task GetSimpleHouseByIdAvailableForUserAsync_HouseNotFound_ThrowsHouseNotFoundException()
        {
            // Arrange
            var houseSimpleInfo = new HouseSimpleInfo()
            {
                Id = ObjectId.GenerateNewId(),
                OwnerId = 123456789,
                MemberIds = new List<decimal>() { 123456789 }
            };

            var exception = new InvalidOperationException("Sequence contains no elements");

            _houseRepositoryMock
                .Setup(hr => hr.FindOneAsync(
                    It.IsAny<PipelineDefinition<HouseInfo, HouseSimpleInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws(exception);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<HouseNotFoundException>(
                () => _houseService.GetSimpleHouseInfoByIdAvailableForUserAsync(houseSimpleInfo.Id, houseSimpleInfo.OwnerId));
        }

        [TestMethod]
        public async Task GetSimpleHouseByIdAvailableForUserAsync_InvalidOperationException_ThrowsException()
        {
            // Arrange
            var houseSimpleInfo = new HouseSimpleInfo()
            {
                Id = ObjectId.GenerateNewId(),
                OwnerId = 123456789,
                MemberIds = new List<decimal>() { 123456789 }
            };

            _houseRepositoryMock
                .Setup(hr => hr.FindOneAsync(
                    It.IsAny<PipelineDefinition<HouseInfo, HouseSimpleInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _houseService.GetSimpleHouseInfoByIdAvailableForUserAsync(houseSimpleInfo.Id, houseSimpleInfo.OwnerId));
        }

        [TestMethod]
        public async Task GetSimpleHouseByIdAvailableForUserAsync_TimeoutException_ThrowsException()
        {
            // Arrange
            var houseSimpleInfo = new HouseSimpleInfo()
            {
                Id = ObjectId.GenerateNewId(),
                OwnerId = 123456789,
                MemberIds = new List<decimal>() { 123456789 }
            };

            _houseRepositoryMock
                .Setup(hr => hr.FindOneAsync(
                    It.IsAny<PipelineDefinition<HouseInfo, HouseSimpleInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws<TimeoutException>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<TimeoutException>(
                () => _houseService.GetSimpleHouseInfoByIdAvailableForUserAsync(houseSimpleInfo.Id, houseSimpleInfo.OwnerId));
        }

        [TestMethod]
        public async Task GetOwnedHouseInfosAsync_ReturnOwnedHouses()
        {
            // Arrange
            var houseSimpleInfos = new List<HouseSimpleInfo>()
            {
                new HouseSimpleInfo()
                {
                    OwnerId = 123456789
                }
            };

            _houseRepositoryMock
                .Setup(hr => hr.FindAsync(
                    It.IsAny<PipelineDefinition<HouseInfo, HouseSimpleInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(houseSimpleInfos);

            // Act
            var result = await _houseService.GetOwnedHouseSimpleInfosOfUserAsync(houseSimpleInfos.First().OwnerId);

            // Assert
            Assert.AreSame(houseSimpleInfos, result);
        }

        [TestMethod]
        public async Task GetMembershipHouseSimpleInfosOfUserAsync_ReturnMembershipHouses()
        {
            // Arrange
            var houseSimpleInfos = new List<HouseSimpleInfo>()
            {
                new HouseSimpleInfo()
                {
                    OwnerId = 123456789
                }
            };

            _houseRepositoryMock
                .Setup(hr => hr.FindAsync(
                    It.IsAny<PipelineDefinition<HouseInfo, HouseSimpleInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(houseSimpleInfos);

            // Act
            var result = await _houseService.GetMembershipHouseSimpleInfosOfUserAsync(houseSimpleInfos.First().OwnerId);

            // Assert
            Assert.AreSame(houseSimpleInfos, result);
        }

        [TestMethod]
        public async Task DeleteHouseByIdAsync_DeletesHouse()
        {
            // Arrange
            var houseInfo = new HouseInfo()
            {
                Id = ObjectId.GenerateNewId(),
                OwnerId = 123456789
            };

            _houseRepositoryMock
                .Setup(hr => hr.DeleteAsync(
                    It.IsAny<FilterDefinition<HouseInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            // Assert
            await _houseService.DeleteHouseByIdOwnedByUserAsync(houseInfo.Id, houseInfo.OwnerId);
        }

        [TestMethod]
        public async Task DeleteHouseByIdAsync_HouseNotFound_ThrowsHouseNotFoundException()
        {
            // Arrange
            var houseInfo = new HouseInfo()
            {
                Id = ObjectId.GenerateNewId(),
                OwnerId = 123456789
            };

            _houseRepositoryMock
                .Setup(hr => hr.DeleteAsync(
                    It.IsAny<FilterDefinition<HouseInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<HouseNotFoundException>(() =>
                _houseService.DeleteHouseByIdOwnedByUserAsync(houseInfo.Id, houseInfo.OwnerId));
        }
    }
}
