using Mbp.EntityFrameworkCore.Domain;
using Mbp.EntityFrameworkCore.PermissionModel;
using Mbp.EntityFrameworkCore.PermissionModel.Enums;
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

        public DbSet<MbpOperationLog> MbpOperationLogs { get; set; }

        public DbSet<MbpDept> MbpDepts { get; set; }

        public DbSet<MbpCategory> MbpCategories { get; set; }

        public DbSet<MbpPosition> MbpPositions { get; set; }

        public DbSet<MbpUserPosition> MbpUserPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 初始化系统管理员
            modelBuilder.Entity<MbpUser>().HasData(
                new MbpUser
                {
                    Id = 1,
                    IsDeleted = false,
                    LoginName = "admin",
                    UserName = "admin",
                    UserStatus = EnumUserStatus.Actived,
                    Password = "94c5fb886bd3cf5f821d239056181a5e",
                    IsAdmin = true
                });

            // 初始化根菜单
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
                    HasChildren = true,
                    SystemCode = "Mbp"
                });

            // 初始化根部门
            modelBuilder.Entity<MbpDept>().HasData(
                new MbpDept
                {
                    Id = 1,
                    DeptCode = "d000001",
                    DeptName = "组织架构",
                    DeptStatus = EnumDeptStatus.Actived,
                    IsDeleted = false,
                    SystemCode = "Mbp",
                    FullDeptName = "组织架构",
                    ParentId = null
                });

            // 初始化根分类
            modelBuilder.Entity<MbpCategory>().HasData(
                new MbpCategory
                {
                    Id = 1,
                    CategoryCode = "f000001",
                    CategoryName = "系统分类",
                    IsDeleted = false,
                    SystemCode = "Mbp",
                    CategoryType = Domain.Enums.EnumCategoryType.Root
                }, new MbpCategory
                {
                    Id = 2,
                    CategoryCode = "f000002",
                    CategoryName = "岗位分类",
                    IsDeleted = false,
                    SystemCode = "Mbp",
                    CategoryType = Domain.Enums.EnumCategoryType.Position,
                    ParentCategoryCode = "f000001",
                    ParentCategoryName = "系统分类"
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
