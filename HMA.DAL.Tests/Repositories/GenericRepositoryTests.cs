using System.Linq;
using System.Threading.Tasks;
using HMA.DAL.Options;
using HMA.DAL.Repositories;
using HMA.DTO.Models.Tests.Test;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace HMA.DAL.Tests.Repositories
{
    [TestClass]
    public class GenericRepositoryTests
    {
        private readonly GenericRepository<TestDalModel> _genericRepository;

        private readonly Mock<IMongoCollection<TestDalModel>> _mongoCollectionMock;
        private readonly Mock<IMongoDatabase> _mongoDatabaseMock;
        private readonly Mock<IMongoClient> _mongoClientMock;

        public GenericRepositoryTests()
        {
            var loggerMock = new Mock<ILogger<GenericRepository<TestDalModel>>>();

            _mongoCollectionMock = new Mock<IMongoCollection<TestDalModel>>();
            _mongoDatabaseMock = new Mock<IMongoDatabase>();
            _mongoClientMock = new Mock<IMongoClient>();
            Setup();

            var mongoRawOptions = new MongoDbOptions();
            var mongoOptions = Microsoft.Extensions.Options.Options.Create(mongoRawOptions);

            _genericRepository = new GenericRepository<TestDalModel>(
                loggerMock.Object,
                _mongoClientMock.Object,
                mongoOptions);
        }

        [TestInitialize]
        public void Setup()
        {
            _mongoCollectionMock.Reset();
            _mongoDatabaseMock.Reset();
            _mongoClientMock.Reset();

            _mongoDatabaseMock
                .Setup(md => md.GetCollection<TestDalModel>(It.IsAny<string>(), null))
                .Returns(_mongoCollectionMock.Object);

            _mongoClientMock
                .Setup(mc => mc.GetDatabase(It.IsAny<string>(), null))
                .Returns(_mongoDatabaseMock.Object);
        }

        [TestMethod]
        public async Task InsertAsync()
        {
            // Arrange
            var model = new TestDalModel()
            {
                Id = ObjectId.GenerateNewId()
            };

            // Act
            model = await _genericRepository.InsertAsync(model);

            // Assert
            Assert.IsNotNull(model.Id);
            Assert.AreNotEqual(BsonObjectId.Empty, model.Id);
        }

        [TestMethod]
        public async Task InsertAsync_IdIsNull_GeneratesNewId()
        {
            // Arrange
            var model = new TestDalModel();

            // Act
            model = await _genericRepository.InsertAsync(model);

            // Assert
            Assert.IsNotNull(model.Id);
            Assert.AreNotEqual(BsonObjectId.Empty, model.Id);
        }

        [TestMethod]
        public async Task InsertAsync_IdIsEmpty_GeneratesNewId()
        {
            // Arrange
            var model = new TestDalModel()
            {
                Id = BsonObjectId.Empty
            };

            // Act
            model = await _genericRepository.InsertAsync(model);

            // Assert
            Assert.IsNotNull(model.Id);
            Assert.AreNotEqual(BsonObjectId.Empty, model.Id);
        }

        [TestMethod]
        public async Task InsertManyAsync()
        {
            // Arrange
            var models = Enumerable
                .Range(0, 5)
                .Select(i => new TestDalModel() { Id = ObjectId.GenerateNewId() })
                .ToList();

            // Act
            models = await _genericRepository.InsertManyAsync(models);

            // Assert
            var allIdsAreNotNullOrEmpty = models.All(m => m.Id != null && m.Id != BsonObjectId.Empty);
            Assert.IsTrue(allIdsAreNotNullOrEmpty);
        }

        [TestMethod]
        public async Task InsertManyAsync_IdIsNull_GeneratesNewId()
        {
            // Arrange
            var models = Enumerable
                .Range(0, 5)
                .Select(i => new TestDalModel())
                .ToList();

            // Act
            models = await _genericRepository.InsertManyAsync(models);

            // Assert
            var allIdsAreNotNullOrEmpty = models.All(m => m.Id != null && m.Id != BsonObjectId.Empty);
            Assert.IsTrue(allIdsAreNotNullOrEmpty);
        }

        [TestMethod]
        public async Task InsertManyAsync_IdIsEmpty_GeneratesNewId()
        {
            // Arrange
            var models = Enumerable
                .Range(0, 5)
                .Select(i => new TestDalModel() { Id = BsonObjectId.Empty })
                .ToList();

            // Act
            models = await _genericRepository.InsertManyAsync(models);

            // Assert
            var allIdsAreNotNullOrEmpty = models.All(m => m.Id != null && m.Id != BsonObjectId.Empty);
            Assert.IsTrue(allIdsAreNotNullOrEmpty);
        }
    }
}
