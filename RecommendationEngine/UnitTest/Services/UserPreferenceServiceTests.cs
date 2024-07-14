using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RecommendationEngine.Common.Exceptions;
using RecommendationEngine.Data;
using RecommendationEngine.Data.Entities;
using RecommendationEngine.Data.Interface;
using RecommendationEngine.Data.Repositories;
using Server.Models.DTO;
using Server.Models.DTO.Profiles;
using Server.Models.Request;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Services
{
    [TestClass]
    public class UserPreferenceServiceTests
    {
        private UserPreferenceService _userPreferenceService;
        private Mock<IUserPreferenceRepository> _userPreferenceRepositoryMock;
        private IMapper _mapper;
        private ILogger<UserPreferenceService> _logger;
        private AppDbContext _context;

        [TestInitialize]
        public void TestInitialize()
        {
            DbContextOptions<AppDbContext> dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "RecommendationEngine").Options;
            _context = new AppDbContext(dbOptions);

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new UserPreferenceProfile()));
            _mapper = config.CreateMapper();

            var repoLogger = Mock.Of<ILogger<UserPreferenceRepository>>();
            var serviceLogger = Mock.Of<ILogger<UserPreferenceService>>();

            var userPreferenceRepository = new UserPreferenceRepository(_context, repoLogger);

            _userPreferenceService = new UserPreferenceService(
                userPreferenceRepository,
                _mapper,
                serviceLogger);
        }

        [TestMethod]
        public async Task Add_ShouldAddUserPreference_WhenValidModelIsProvided()
        {
            // Arrange
            var model = new UserPreferenceModel { UserId = 1, CharacteristicId = 1 };

            // Act
            var result = await _userPreferenceService.Add(model);

            // Assert
            var userPreference = await _context.UserPreferences.FirstOrDefaultAsync(up => up.UserId == 1 && up.CharacteristicId == 1);
            Assert.IsNotNull(userPreference);
        }

        [TestMethod]
        public async Task Add_ShouldThrowException_WhenDuplicateUserPreferenceIsProvided()
        {
            // Arrange
            var model = new UserPreferenceModel { UserId = 1, CharacteristicId = 1 };
            _context.UserPreferences.Add(new UserPreference { UserId = 1, CharacteristicId = 1 });
            await _context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<AppException>(async () => await _userPreferenceService.Add(model));
        }

        //[TestMethod]
        //public async Task GetPreferencesByUserId_ShouldReturnPreferences_WhenUserIdIsValid()
        //{
        //    // Arrange
        //    _context.UserPreferences.Add(new UserPreference { UserId = 2, CharacteristicId = 5 });
        //    _context.UserPreferences.Add(new UserPreference {UserId = 2, CharacteristicId = 2 });
        //    await _context.SaveChangesAsync();

        //    // Act
        //    var result = await _userPreferenceService.GetPreferencesByUserId(2);

        //    // Assert
        //    Assert.AreEqual(2, result.Count);
        //}

        
    }
}
