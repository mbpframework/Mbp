using Medical.Ai.Mbdp.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Mbp.AspNetCore.Mvc.Convention;
using System;
using System.Collections.Generic;
using System.Text;
using Medical.Ai.Mbdp.Domain.DomainEntities.Demo;
using Medical.Ai.Mbdp.Domain;
using Mbp.Core.Modularity;
using Medical.Ai.Mbdp.EntityFrameworkCore.EntityFrameworkCore;
using System.Linq;
using Mbp.Core.Core;
using Microsoft.EntityFrameworkCore;
using Medical.Ai.Mbdp.Application.Contracts.Demo.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Mbp.Authentication.JwtBearer;
using System.Threading.Tasks;
using Mbp.Core.Core.System;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Medical.Ai.Mbdp.Application.Demo
{
    [Authorize("GlobalPermission")]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class DemoAppService : IDemoAppService
    {
        private readonly IDemoDomainService _demoDomainService = AutofacService.Resolve<IDemoDomainService>();

        //OOM映射服务
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        private readonly IJwtBearerService _jwtBearerService = null;

        private readonly IConfiguration _config;

        private readonly ILogger<DemoAppService> _logger;

        public DemoAppService(DefaultDbContext defaultDbContext, IJwtBearerService jwtBearerService, IConfiguration config, ILogger<DemoAppService> logger)
        {
            _defaultDbContext = defaultDbContext;
            _jwtBearerService = jwtBearerService;
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// 获取所有的博客
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBlogs")]
        [AutoAop(IsTranstion = false)]
        public virtual List<BlogDto> GetBlogs()
        {
            _logger.LogInformation("我在写日志喔");

            List<Blog> blogs = _defaultDbContext.Blogs.Include(b => b.Posts).ToList();

            return _mapper.Map<List<BlogDto>>(blogs);
        }

        /// <summary>
        /// 删除博客
        /// </summary>
        /// <param name="blogDto"></param>
        [HttpDelete("DeleteBlog")]
        public virtual void Delete(int Id)
        {
            var blog = _defaultDbContext.Blogs.Include(b => b.Posts).Where(b => b.Id == Id).First();
            _defaultDbContext.Blogs.Remove(blog);

            _defaultDbContext.SaveChanges();
        }

        [HttpPost("AddBlog")]
        public virtual void AddBlog(BlogDto blogDto)
        {
            var blog = _mapper.Map<Blog>(blogDto);
            _defaultDbContext.Blogs.Add(blog);

            _defaultDbContext.SaveChanges();
        }

        [AllowAnonymous]
        [HttpGet("GetToken")]
        public async virtual Task<Jwt> GetToken()
        {
            // todo 取出用户的角色
            var t = await _jwtBearerService.CreateJwt("123", "lixp", new List<Claim>()
            {
               new Claim(ClaimTypes.Role, "admin")
            });
            return t;
        }
    }
}
