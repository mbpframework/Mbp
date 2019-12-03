using Mbp.EntityFrameworkCore.PermissionModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.EntityFrameworkCore
{
    public class MbpDbContext<TDbContext> : DbContext
        where TDbContext : DbContext
    {
        public MbpDbContext(DbContextOptions<TDbContext> options) : base(options)
        {

        }

        public DbSet<MbpRole> MbpRoles { get; set; }

        public DbSet<MbpMenuClaims>  MbpMenuClaims { get; set; }

        public DbSet<MbpUser> MbpUsers { get; set; }

        public DbSet<MbpUserRole> MbpUserRoles { get; set; }

        public DbSet<MbpMenu> MbpMenus { get; set; }

        public DbSet<MbpRoleMenu> MbpRoleMenus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MbpUser>().HasData(
                new MbpUser
                {
                    Id = 1,
                    IsDeleted = false,
                    LoginName = "admin",
                    UserName = "admin",
                    UserStatus = EnumUserStatus.Actived,
                    IsAdmin = true
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
