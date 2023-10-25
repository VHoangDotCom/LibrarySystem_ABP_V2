using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.EntityFrameworkCore
{
    public static class LibrarySystemDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<LibrarySystemDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<LibrarySystemDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
