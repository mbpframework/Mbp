using Mbp.Ddd.Application.Mbp.Dto;
using Mbp.EntityFrameworkCore.PermissionModel.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.AccountService.Dto
{
    public class DeptInputDto : DtoBase
    {
        /// <summary>
        /// 部门编号
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [JsonProperty("name")]
        public string DeptName { get; set; }

        /// <summary>
        /// 部门名称全称
        /// </summary>
        public string FullDeptName { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        public string DeptCode { get; set; }

        /// <summary>
        /// 上级部门名称
        /// </summary>
        public string ParentDeptName { get; set; }

        /// <summary>
        /// 上级部门编号
        /// </summary>
        public string ParentDeptCode { get; set; }

        /// <summary>
        /// 上级部门Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 部门状态
        /// </summary>
        public EnumDeptStatus DeptStatus { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 部门层级
        /// </summary>
        public int Level { get; set; }

        public byte[] ConcurrencyStamp { get; set; }
    }
}
