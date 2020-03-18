using Mbp.Ddd.Application.Mbp.Dto;
using Mbp.EntityFrameworkCore.PermissionModel;
using Mbp.EntityFrameworkCore.PermissionModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace EMS.Application.Contracts.AccountService.Dto
{
    public class MenuOutputDto : DtoBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        public string Code { get; set; }

        public int Level { get; set; }

        public string Path { get; set; }

        public int ParentId { get; set; }

        public int Order { get; set; }

        public EnumMenuType MenuType { get; set; }

        [JsonProperty("component")]
        public string MenuCompent { get; set; }

        [JsonProperty("icon")]
        public string MenuIcon { get; set; }

        /// <summary>
        /// 下级菜单
        /// </summary>
        [JsonProperty("children")]
        public List<MenuOutputDto> Children { get; private set; } = new List<MenuOutputDto>();

        public byte[] ConcurrencyStamp { get; set; }
    }
}
