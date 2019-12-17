using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mbp.Ddd.Application.Mbp.UI
{
    /// <summary>
    /// 查询参数类
    /// </summary>
    /// <typeparam name="TSearch"></typeparam>
    public class SearchOptions<TSearch> : DtoBase where TSearch : class, new()
    {
        [Required]
        public int PageIndex { get; set; } = 1;

        [Required]
        public int PageSize { get; set; } = 20;

        public TSearch Search { get; set; } = new TSearch();
    }
}
