using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mbp.Core.Modularity;
using Medical.Ai.Mbdp.Application.Contracts.Demo.Dto;
using Medical.Ai.Mbdp.Domain.DomainEntities.Demo;
using Medical.Ai.Mbdp.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medical.Ai.Mbdp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        //OOM映射服务
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        public DemoController(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
            //_demoDomainService = demoDomainService;
        }

        /// <summary>
        /// 获取所有的博客
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Blog> GetBlogs()
        {
            List<Blog> blogs = _defaultDbContext.Blogs.Include(b => b.Posts).ToList();

            var list = _mapper.Map<List<BlogDto>>(blogs);

            //return _mapper.Map<List<BlogDto>>(blogs);

            return blogs;
        }

        /// <summary>
        /// 删除博客
        /// </summary>
        /// <param name="blogDto"></param>
        [HttpDelete]
        public void Delete(Guid Id)
        {
            var blog = _defaultDbContext.Blogs.Find(Id);
            _defaultDbContext.Blogs.Remove(blog);

            _defaultDbContext.SaveChanges();
        }

        [HttpPost]
        public void AddBlog(BlogDto blogDto)
        {
            var blog = _mapper.Map<Blog>(blogDto);
            _defaultDbContext.Blogs.Add(blog);

            _defaultDbContext.SaveChanges();
        }
    }
}