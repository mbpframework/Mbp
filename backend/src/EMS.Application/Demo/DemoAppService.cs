using EMS.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using Mbp.AspNetCore.Mvc.Convention;
using System;
using System.Collections.Generic;
using System.Text;
using EMS.Domain.DomainEntities.Demo;
using EMS.Domain;
using Mbp.Core.Modularity;
using EMS.EntityFrameworkCore.EntityFrameworkCore;
using System.Linq;
using Mbp.Core.Core;
using Microsoft.EntityFrameworkCore;
using EMS.Application.Contracts.Demo.Dto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Mbp.Authentication.JwtBearer;
using System.Threading.Tasks;
using Mbp.Core.Core.System;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EMS.Application.Demo
{
    //[Authorize("GlobalPermission")]
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

        [HttpPut("UpdateBlog")]
        public virtual void UpdateBlog(BlogDto blogDto)
        {
            var blog = _defaultDbContext.Blogs.Include(b => b.Posts).Where(b => b.Id == blogDto.Id).First();
            blog.Url = blogDto.Url;
            _defaultDbContext.Blogs.Update(blog);

            try
            {
                _defaultDbContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Blog)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            // 当前值
                            var proposedValue = proposedValues[property];

                            // 数据库值
                            var databaseValue = databaseValues[property];

                            // TODO: decide which value should be written to database
                            // proposedValues[property] = <value to be saved>;
                        }

                        // Refresh original values to bypass next concurrency check
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
            }

        }

        [HttpPut("UpdateBlog2")]
        public virtual void UpdateBlog2(BlogDto blogDto)
        {
            var blog = _defaultDbContext.Blogs.Include(b => b.Posts).Where(b => b.Id == blogDto.Id).First();
            blog.Url = blogDto.Url;
            _defaultDbContext.Blogs.Update(blog);

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
