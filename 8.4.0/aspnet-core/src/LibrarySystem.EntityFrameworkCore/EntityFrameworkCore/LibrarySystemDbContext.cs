﻿using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using LibrarySystem.Authorization.Roles;
using LibrarySystem.Authorization.Users;
using LibrarySystem.MultiTenancy;
using LibrarySystem.Entities;

namespace LibrarySystem.EntityFrameworkCore
{
    public class LibrarySystemDbContext : AbpZeroDbContext<Tenant, Role, User, LibrarySystemDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<CloudFolder> CloudFolders { get; set; }
        public DbSet<CloudFile> CloudFiles { get; set; }

        public LibrarySystemDbContext(DbContextOptions<LibrarySystemDbContext> options)
            : base(options)
        {
        }
    }
}
