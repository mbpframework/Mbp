using AutoMapper;
using Medical.Ai.Mbdp.Application.Contracts.Demo.Dto;
using Medical.Ai.Mbdp.Domain.DomainEntities.Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical.Ai.Mbdp.Application
{
    /// <summary>
    /// object to object Map
    /// </summary>
    public static class AutoMapperConfig
    {
        public static IMapper CreateMapper()
        {
            // TO DO 在这里注册所有的类型映射cfg.CreateMap<TSource, TDestination>()
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Blog, BlogDto>();
                cfg.CreateMap<Post, PostDto>();

                cfg.CreateMap<BlogDto, Blog>();
                cfg.CreateMap<PostDto, Post>();

                // to do
            });

            return config.CreateMapper();
        }
    }
}
