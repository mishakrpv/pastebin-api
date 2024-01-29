using Core.Entities;
using Infrastructure.Data;
using IntegrationTests.Builders;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Repositories
{
    public class ObjectDetailsRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly EfRepository<ObjectDetails> _objectDetailsRepository;
        private readonly ObjectDetailsBuilder _objectDetailsBuilder = new();

        public ObjectDetailsRepositoryTests()
        {
            var dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(dbOptions);
            _objectDetailsRepository = new EfRepository<ObjectDetails>(_context);
        }

        [Fact]
        public async Task GetsExistingObjectDetails()
        {
            var existingObjectDetails = _objectDetailsBuilder.WithDefaultValues();
            _context.ObjectDetails.Add(existingObjectDetails);
            _context.SaveChanges();
            string key = existingObjectDetails.Key;

            var objectDetailsFromRepo = await _objectDetailsRepository.GetByIdAsync(key);
            Assert.Equal(_objectDetailsBuilder.TestKey, objectDetailsFromRepo!.Key);
        }
    }
}
