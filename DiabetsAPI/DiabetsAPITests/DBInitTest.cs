using DiabetsAPI.DB;
using Microsoft.EntityFrameworkCore;

namespace DiabetsAPITests
{
    public class DBInitTest
    {
        private DbContextOptions<DiabetsContext> dbContextOptions = new DbContextOptionsBuilder<DiabetsContext>()
            .UseInMemoryDatabase(databaseName: "PrimeDb")
            .Options;

        [Fact]
        public void TestInit()
        {
            DiabetsContext diabetsContext = new DiabetsContext(dbContextOptions);
            DbInitializer.Initialize(diabetsContext);
            Assert.True(diabetsContext.Database.CanConnect());
        }
    }
}
