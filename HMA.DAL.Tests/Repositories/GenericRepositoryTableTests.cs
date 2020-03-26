using HMA.DAL.Options;
using HMA.DAL.Repositories;
using HMA.DTO.Models.Tests.Test;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;

namespace HMA.DAL.Tests.Repositories
{
    [TestClass]
    public class GenericRepositoryTableTests
    {
        [TestMethod]
        public void GetCollectionName_ReturnsTableAttributeValue()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GenericRepository<TestDalModel>>>();

            var mongoCollection = new Mock<IMongoCollection<TestDalModel>>();

            var mongoDatabase = new Mock<IMongoDatabase>();
            mongoDatabase
                .Setup(md => md.GetCollection<TestDalModel>(It.IsAny<string>(), null))
                .Returns(mongoCollection.Object);

            var mongoClientMock = new Mock<IMongoClient>();
            mongoClientMock
                .Setup(mc => mc.GetDatabase(It.IsAny<string>(), null))
                .Returns(mongoDatabase.Object);

            var mongoRawOptions = new MongoDbOptions();
            var mongoOptions = Microsoft.Extensions.Options.Options.Create(mongoRawOptions);

            // Act
            var genericRepository = new GenericRepository<TestDalModel>(
                loggerMock.Object,
                mongoClientMock.Object,
                mongoOptions);

            // Assert
            Assert.AreEqual(0, loggerMock.Invocations.Count);
        }

        [TestMethod]
        public void GetCollectionName_NoTableAttribute_ReturnsDefaultCollectionName()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<GenericRepository<TestDalModelWithoutTableAttribute>>>();

            var mongoCollection = new Mock<IMongoCollection<TestDalModelWithoutTableAttribute>>();

            var mongoDatabase = new Mock<IMongoDatabase>();
            mongoDatabase
                .Setup(md => md.GetCollection<TestDalModelWithoutTableAttribute>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                .Returns(mongoCollection.Object);

            var mongoClientMock = new Mock<IMongoClient>();
            mongoClientMock
                .Setup(mc => mc.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()))
                .Returns(mongoDatabase.Object);

            var mongoRawOptions = new MongoDbOptions();
            var mongoOptions = Microsoft.Extensions.Options.Options.Create(mongoRawOptions);

            // Act
            var genericRepository = new GenericRepository<TestDalModelWithoutTableAttribute>(
                loggerMock.Object,
                mongoClientMock.Object,
                mongoOptions);

            // Assert
            Assert.AreEqual(1, loggerMock.Invocations.Count);
        }
    }
}
