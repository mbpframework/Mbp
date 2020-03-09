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

        public DbSet<MbpMenuClaim> MbpMenuClaims { get; set; }

        public DbSet<MbpUser> MbpUsers { get; set; }

        public DbSet<MbpUserRole> MbpUserRoles { get; set; }

        public DbSet<MbpMenu> MbpMenus { get; set; }

        public DbSet<MbpRoleMenu> MbpRoleMenus { get; set; }

        public DbSet<MbpUserClaim> MbpUserClaims { get; set; }

        public DbSet<MbpOperationLog>  MbpOperationLogs { get; set; }

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
                    Password= "94c5fb886bd3cf5f821d239056181a5e",
                    IsAdmin = true
                });

            modelBuilder.Entity<MbpMenu>().HasData(
                new MbpMenu
                {
                    Id = 1,
                    IsDeleted = false,
                    Code = "root",
                    Level = 1,
                    Name = "Mbp平台",
                    Order = 1,
                    ParentId = 0,
                    Path = "/",
                    CodePath = "root",
                    HasChildren = true
                }, new MbpMenu
                {
                    Id = 2,
                    IsDeleted = false,
                    Code = "m10001",
                    Level = 2,
                    Name = "数据建模系统",
                    Order = 1,
                    ParentId = 1,
                    Path = "/",
                    CodePath = "root/m10001",
                    SystemCode = "mdp",
                    HasChildren = true
                }, new MbpMenu
                {
                    Id = 3,
                    IsDeleted = false,
                    Code = "m20001",
                    Level = 2,
                    Name = "大数据系统",
                    Order = 1,
                    ParentId = 1,
                    Path = "/",
                    CodePath = "root/m20001",
                    SystemCode = "mbdp",
                    HasChildren = true
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
