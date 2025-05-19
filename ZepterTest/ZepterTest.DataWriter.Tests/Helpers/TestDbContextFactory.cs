using Microsoft.EntityFrameworkCore;
using ZepterTest.Common;

namespace ZepterTest.DataWriter.Tests.Helpers
{
    /// <summary>
    /// Factory for creating test database contexts
    /// </summary>
    public static class TestDbContextFactory
    {
        /// <summary>
        /// Creates a new in-memory database context for testing
        /// </summary>
        /// <param name="databaseName">The name of the in-memory database</param>
        /// <returns>A new instance of ZepterTestContext</returns>
        public static ZepterTestContext CreateDbContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ZepterTestContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new ZepterTestContext(options);
        }
    }
}
