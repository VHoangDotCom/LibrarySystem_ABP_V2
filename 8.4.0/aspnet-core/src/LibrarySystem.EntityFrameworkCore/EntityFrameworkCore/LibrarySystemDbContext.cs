using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using LibrarySystem.Authorization.Roles;
using LibrarySystem.Authorization.Users;
using LibrarySystem.MultiTenancy;

namespace LibrarySystem.EntityFrameworkCore
{
    public class LibrarySystemDbContext : AbpZeroDbContext<Tenant, Role, User, LibrarySystemDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public LibrarySystemDbContext(DbContextOptions<LibrarySystemDbContext> options)
            : base(options)
        {
        }
    }
}
