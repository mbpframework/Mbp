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

        public DbSet<MbpRoleClaims> MbpRoleClaims { get; set; }

        public DbSet<MbpUser> MbpUsers { get; set; }

        public DbSet<MbpUserRole> MbpUserRoles { get; set; }

        public DbSet<MbpUserClaims> MbpUserClaims { get; set; }

        public DbSet<MbpMenu> MbpMenus { get; set; }

        public DbSet<MbpRoleMenu> MbpRoleMenus { get; set; }
    }
}
