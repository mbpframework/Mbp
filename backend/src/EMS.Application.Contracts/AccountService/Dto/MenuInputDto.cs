using Mbp.Ddd.Application.Mbp.Dto;
using Mbp.EntityFrameworkCore.PermissionModel;
using Mbp.EntityFrameworkCore.PermissionModel.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Application.Contracts.AccountService.Dto
{
    public class MenuInputDto : DtoBase
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public string CodePath { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public int ParentId { get; set; }

        [Required]
        public int Order { get; set; }

        public string SystemCode { get; set; }

        [Required]
        public EnumMenuType MenuType { get; set; }

        [Required]
        [JsonProperty("component")]
        public string MenuCompent { get; set; }

        [JsonProperty("icon")]
        public string MenuIcon { get; set; }

        public byte[] ConcurrencyStamp { get; set; }
    }
}
