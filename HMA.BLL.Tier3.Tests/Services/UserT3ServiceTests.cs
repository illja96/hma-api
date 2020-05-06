using System.Threading;
using System.Threading.Tasks;
using HMA.AutoMapper.Tests.Factory;
using HMA.BLL.Tier2.Services.Interfaces;
using HMA.BLL.Tier3.Services;
using HMA.DTO.Models.Tests.Stubs;
using HMA.DTO.Models.User;
using HMA.DTO.ViewModels.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HMA.BLL.Tier3.Tests.Services
{
    [TestClass]
    public class UserT3ServiceTests
    {
        private readonly UserT3Service _userT3Service;

        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IUserT2Service> _userT2Service;

        public UserT3ServiceTests()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _userT2Service = new Mock<IUserT2Service>();

            var mapper = AutoMapperFactory.Create();

            _userT3Service = new UserT3Service(
                mapper,
                _httpContextAccessorMock.Object,
                _userT2Service.Object);
        }

        [TestInitialize]
        public void Setup()
        {
            _httpContextAccessorMock.Reset();
            _userT2Service.Reset();
        }

        [TestMethod]
        public async Task RegisterAsync_ReturnsOkObjectResult()
        {
            // Arrange
            _httpContextAccessorMock
                .Setup(hca => hca.HttpContext)
                .Returns(HttpContextStubs.HttpContextWithUser);

            var userInfo = new UserInfo();

            _userT2Service
                .Setup(ut2s => ut2s.RegisterAsync(
                    It.IsAny<UserInfo>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(userInfo);

            // Act
            var result = await _userT3Service.RegisterAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(result.Value, typeof(UserInfoViewModel));
        }

        [TestMethod]
        public async Task GetCurrentAsync_ReturnsOkObjectResult()
        {
            // Arrange
            _httpContextAccessorMock
                .Setup(hca => hca.HttpContext)
                .Returns(HttpContextStubs.HttpContextWithUser);

            var userInfo = new UserInfo();

            _userT2Service
                .Setup(ut2s => ut2s.GetByIdAsync(
                    It.IsAny<decimal>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(userInfo);

            // Act
            var result = await _userT3Service.GetCurrentAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(result.Value, typeof(UserInfoViewModel));
        }

        [TestMethod]
        public async Task UpdateCurrentAsync_ReturnsOkObjectResult()
        {
            // Arrange
            _httpContextAccessorMock
                .Setup(hca => hca.HttpContext)
                .Returns(HttpContextStubs.HttpContextWithUser);

            var userInfo = new UserInfo();

            _userT2Service
                .Setup(ut2s => ut2s.UpdateAsync(
                    It.IsAny<UserInfo>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(userInfo);

            // Act
            var result = await _userT3Service.UpdateCurrentAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsInstanceOfType(result.Value, typeof(UserInfoViewModel));
        }

        [TestMethod]
        public async Task DeleteProfileAsync_ReturnsStatusCodeResult()
        {
            // Arrange
            _httpContextAccessorMock
                .Setup(hca => hca.HttpContext)
                .Returns(HttpContextStubs.HttpContextWithUser);
            
            // Act
            var result = await _userT3Service.DeleteProfileAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNull(result.Value, null);
        }
    }
}
