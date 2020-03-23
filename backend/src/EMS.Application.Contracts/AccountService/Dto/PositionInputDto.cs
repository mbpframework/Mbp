using Mbp.EntityFrameworkCore.Domain.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.AccountService.Dto
{
    public class PositionInputDto
    {
        /// <summary>
        /// 岗位编号
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 岗位名称
        /// </summary>
        [JsonProperty("name")]
        public string PositionName { get; set; }

        /// <summary>
        /// 岗位名称全称
        /// </summary>
        public string FullPositionName { get; set; }

        /// <summary>
        /// 岗位编号
        /// </summary>
        public string PositionCode { get; set; }

        /// <summary>
        /// 岗位类别
        /// </summary>
        public EnumPositionType PositionType { get; set; }

        /// <summary>
        /// 上级岗位名称
        /// </summary>
        public string ParentPositionName { get; set; }

        /// <summary>
        /// 上级岗位编号
        /// </summary>
        public string ParentPositionCode { get; set; }

        /// <summary>
        /// 上级岗位Id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 岗位状态
        /// </summary>
        public EnumPositionStatus PositionStatus { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 岗位层级
        /// </summary>
        public int Level { get; set; }

        public byte[] ConcurrencyStamp { get; set; }
    }
}
