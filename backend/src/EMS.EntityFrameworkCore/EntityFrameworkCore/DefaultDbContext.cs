using Mbp.EntityFrameworkCore;
using EMS.Domain.DomainEntities.Demo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.EntityFrameworkCore.EntityFrameworkCore
{
    /// <summary>
    /// 默认的DbContext
    /// </summary>
    public class DefaultDbContext : MbpDbContext<DefaultDbContext>

    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {

        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
