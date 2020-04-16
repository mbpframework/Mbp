using Mbp.EntityFrameworkCore;
using EMS.Domain.DomainEntities.Demo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using EMS.Domain.DomainEntities.Base;
using EMS.Domain.DomainEntities.Train;
using EMS.Domain.DomainEntities.Train.Plan;

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

        public DbSet<EmsTrainSubject> EmsTrainSubjects { get; set; }

        public DbSet<EmsTrainPlanWeek>  EmsTrainPlanWeeks { get; set; }

        public DbSet<EmsTrainPlanWeekDetail> EmsTrainPlanWeekDetails { get; set; }

        public DbSet<EmsTrainRecord> EmsTrainRecords { get; set; }

        public DbSet<EmsTrainReport> EmsTrainReports { get; set; }

        public DbSet<EmsTrainScore> EmsTrainScores { get; set; }

        public DbSet<EmsTrainStatistics> EmsTrainStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
