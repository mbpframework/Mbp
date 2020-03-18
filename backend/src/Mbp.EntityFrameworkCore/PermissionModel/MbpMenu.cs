using Mbp.Core.Entity;
using Mbp.Core.Entity.Aggregate;
using Mbp.EntityFrameworkCore.PermissionModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    /// <summary>
    /// 菜单表,菜单可以指示一个页面,一个菜单下面可以包含很多操作,就比如页面上很多操作一样
    /// </summary>
    public class MbpMenu : AggregateBase<int>, ISoftDelete
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public int Order { get; set; }

        /// <summary>
        /// 主要用作菜单层级查询
        /// </summary>
        public string CodePath { get; set; }

        /// <summary>
        /// 菜单等级,1,2,3
        /// </summary>
        public int Level { get; set; }

        public string Path { get; set; }

        public int ParentId { get; set; }

        public bool IsDeleted { get; set; }

        public List<MbpMenuClaim> MenuClaims { get; set; }

        public string SystemCode { get; set; }

        public bool HasChildren { get; set; }

        public string MenuCompent { get; set; }

        public string MenuIcon { get; set; }

        public bool IsEnabled { get; set; }

        /// <summary>
        /// 菜单的类型,页面和按钮
        /// </summary>
        public EnumMenuType MenuType { get; set; }
    }
}
