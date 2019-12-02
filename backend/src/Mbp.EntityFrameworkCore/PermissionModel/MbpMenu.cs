using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    /// <summary>
    /// 菜单表,菜单可以指示一个页面,一个菜单下面可以包含很多操作,就比如页面上很多操作一样
    /// </summary>
    public class MbpMenu : EntityBase<int>, ISoftDelete
    {
        public string Name { get; set; }

        public string Code { get; set; }

        /// <summary>
        /// 菜单等级,1,2,3
        /// </summary>
        public int Level { get; set; }

        public string Path { get; set; }

        public int ParentId { get; set; }

        public bool IsDeleted { get; set; }

        public List<MbpRoleClaims> RoleClaims { get; set; }

        [MaxLength(256)]
        public string ConcurrencyStamp { get; set; }
    }
}
