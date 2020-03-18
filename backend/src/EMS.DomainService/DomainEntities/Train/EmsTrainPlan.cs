using Mbp.Core.Entity;
using Mbp.EntityFrameworkCore.PermissionModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Domain.DomainEntities.Train
{
    /// <summary>
    /// 训练计划
    /// </summary>
    public class EmsTrainPlan : EntityBase<int>, ISoftDelete
    {
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public MbpUser User { get; set; }

        [MaxLength(256)]
        public string UserName { get; set; }

        public string Major { get; set; }

        public bool IsDeleted { get; set; }
    }
}
