using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HMA.BLL.Tier1.Exceptions.User;
using HMA.BLL.Tier1.Services;
using HMA.DAL.Repositories.Interfaces;
using HMA.DTO.Models.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;

namespace HMA.BLL.Tier1.Tests.Services
{
    [TestClass]
    public class UserT1ServiceTests
    {
        private readonly UserT1Service _userService;

        private readonly Mock<IGenericRepository<UserInfo>> _userRepositoryMock;

        public UserT1ServiceTests()
        {
            _userRepositoryMock = new Mock<IGenericRepository<UserInfo>>();

            _userService = new UserT1Service(_userRepositoryMock.Object);
        }

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock.Reset();
        }

        [TestMethod]
        public async Task ExistsAsync_UserExists_ReturnsTrue()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789
            };

            _userRepositoryMock
                .Setup(ur => ur.CountAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var isUserExists = await _userService.ExistsAsync(userInfo.GoogleId);

            // Arrange
            Assert.IsTrue(isUserExists);
        }

        [TestMethod]
        public async Task ExistsAsync_UserNotExists_ReturnsFalse()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789
            };

            _userRepositoryMock
                .Setup(ur => ur.CountAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            var isUserExists = await _userService.ExistsAsync(userInfo.GoogleId);

            // Arrange
            Assert.IsFalse(isUserExists);
        }

        [TestMethod]
        public async Task GetByIdAsync_ReturnsUser()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789
            };

            _userRepositoryMock
                .Setup(ur => ur.FindOneAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(userInfo);

            // Act
            var result = await _userService.GetByIdAsync(userInfo.GoogleId);

            // Assert
            Assert.AreSame(userInfo, result);
        }

        [TestMethod]
        public async Task GetByIdAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789
            };

            var exception = new InvalidOperationException("Sequence contains no elements");

            _userRepositoryMock
                .Setup(ur => ur.FindOneAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws(exception);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<UserNotFoundException>(() =>
                _userService.GetByIdAsync(userInfo.GoogleId));
        }

        [TestMethod]
        public async Task GetByIdAsync_InvalidOperationException_ThrowsException()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789
            };

            _userRepositoryMock
                .Setup(ur => ur.FindOneAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _userService.GetByIdAsync(userInfo.GoogleId));
        }

        [TestMethod]
        public async Task GetByIdAsync_TimeoutException_ThrowsException()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789
            };

            _userRepositoryMock
                .Setup(ur => ur.FindOneAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws<TimeoutException>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<TimeoutException>(() =>
                _userService.GetByIdAsync(userInfo.GoogleId));
        }

        [TestMethod]
        public async Task GetByEmailAsync_ReturnsUser()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789,
                Email = "test@gmail.com"
            };

            _userRepositoryMock
                .Setup(ur => ur.FindOneAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(userInfo);

            // Act
            var result = await _userService.GetByEmailAsync(userInfo.Email);

            // Assert
            Assert.AreSame(userInfo, result);
        }

        [TestMethod]
        public async Task GetByEmailAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789
            };

            var exception = new InvalidOperationException("Sequence contains no elements");

            _userRepositoryMock
                .Setup(ur => ur.FindOneAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws(exception);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<UserNotFoundException>(() =>
                _userService.GetByEmailAsync(userInfo.Email));
        }

        [TestMethod]
        public async Task GetByEmailAsync_InvalidOperationException_ThrowsException()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789
            };

            _userRepositoryMock
                .Setup(ur => ur.FindOneAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                _userService.GetByEmailAsync(userInfo.Email));
        }

        [TestMethod]
        public async Task GetByEmailAsync_TimeoutException_ThrowsException()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789
            };

            _userRepositoryMock
                .Setup(ur => ur.FindOneAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws<TimeoutException>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<TimeoutException>(() =>
                _userService.GetByEmailAsync(userInfo.Email));
        }

        [TestMethod]
        public async Task GetByIdsAsync_ReturnUsers()
        {
            // Arrange
            var userSimpleInfos = new List<UserSimpleInfo>()
            {
                new UserSimpleInfo()
                {
                    GoogleId = 123456789
                }
            };

            var userIds = userSimpleInfos
                .Select(usi => usi.GoogleId)
                .ToList();

            _userRepositoryMock
                .Setup(ur => ur.FindAsync(
                    It.IsAny<PipelineDefinition<UserInfo, UserSimpleInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(userSimpleInfos);

            // Act
            var result = await _userService.GetByIdsAsync(userIds);

            // Assert
            Assert.AreSame(userSimpleInfos, result);
        }

        [TestMethod]
        public async Task RegisterAsync_RegisterUser()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789,
                Email = "test@gmail.com",
                EmailVerified = true
            };

            _userRepositoryMock
                .Setup(ur => ur.CountAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _userRepositoryMock
                .Setup(ur => ur.InsertAsync(
                    It.IsAny<UserInfo>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserInfo ur, CancellationToken ct) => userInfo);

            // Act
            var result = await _userService.RegisterAsync(userInfo);

            // Assert
            Assert.AreSame(userInfo, result);
            Assert.AreEqual(userInfo.RegistrationDate, userInfo.LastUpdateDate);

            var isRegistrationDateNearUtcNow = DateTime.UtcNow.AddMinutes(-1) < userInfo.RegistrationDate &&
                                               userInfo.RegistrationDate < DateTime.Now.AddMinutes(1);
            Assert.IsTrue(isRegistrationDateNearUtcNow);
        }

        [TestMethod]
        public async Task RegisterAsync_EmailIsNotVerified_ThrowsUserEmailNotVerifiedException()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789,
                Email = "test@gmail.com",
                EmailVerified = false
            };

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<UserEmailNotVerifiedException>(
                () => _userService.RegisterAsync(userInfo));
        }

        [TestMethod]
        public async Task RegisterAsync_UserAlreadyRegistered_ThrowsUserDuplicateInsertionException()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789,
                Email = "test@gmail.com",
                EmailVerified = true
            };

            _userRepositoryMock
                .Setup(ur => ur.CountAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<UserDuplicateInsertionException>(
                () => _userService.RegisterAsync(userInfo));
        }

        [TestMethod]
        public async Task UpdateAsync_UpdatesUserInfo()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789,
                Email = "test@gmail.com",
                EmailVerified = true
            };

            _userRepositoryMock
                .Setup(ur => ur.UpdateOneAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<UpdateDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(userInfo);

            _userRepositoryMock
                .Setup(ur => ur.FindOneAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(userInfo);

            // Act
            var result = await _userService.UpdateAsync(userInfo);

            // Assert
            Assert.AreSame(userInfo, result);

            var isLastUpdateDateNearUtcNow = DateTime.UtcNow.AddMinutes(-1) < userInfo.LastUpdateDate &&
                                             userInfo.LastUpdateDate < DateTime.Now.AddMinutes(1);
            Assert.IsTrue(isLastUpdateDateNearUtcNow);
        }

        [TestMethod]
        public async Task UpdateAsync_EmailIsNotVerified_ThrowsUserEmailNotVerifiedException()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789,
                Email = "test@gmail.com",
                EmailVerified = false
            };

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<UserEmailNotVerifiedException>(
                () => _userService.UpdateAsync(userInfo));
        }

        [TestMethod]
        public async Task UpdateAsync_UserNotFound_ThrowsException()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789,
                Email = "test@gmail.com",
                EmailVerified = true
            };

            // TODO: Check with exception throws when no single model found to update
            _userRepositoryMock
                .Setup(ur => ur.UpdateOneAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<UpdateDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _userService.UpdateAsync(userInfo));
        }

        [TestMethod]
        public async Task DeleteByIdAsync_DeletesUser()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789
            };

            _userRepositoryMock
                .Setup(ur => ur.DeleteAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            // Assert
            await _userService.DeleteByIdAsync(userInfo.GoogleId);
        }

        [TestMethod]
        public async Task DeleteByIdAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            var userInfo = new UserInfo()
            {
                GoogleId = 123456789
            };

            _userRepositoryMock
                .Setup(ur => ur.DeleteAsync(
                    It.IsAny<FilterDefinition<UserInfo>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            // Act
            // Assert
            await Assert.ThrowsExceptionAsync<UserNotFoundException>(
                () => _userService.DeleteByIdAsync(userInfo.GoogleId));
        }
    }
}
