using Mbp.Ddd.Application.Mbp.Dto;

namespace Medical.Ai.Mbdp.Application.Contracts.AccountService.DtoSearch
{
    public class MenuSearchOptions
    {
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string SystemCode { get; set; } = string.Empty;
    }
}
