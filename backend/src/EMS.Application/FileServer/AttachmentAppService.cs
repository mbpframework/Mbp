using AutoMapper;
using EMS.Application.Contracts.FileServer;
using EMS.Application.Contracts.FileServer.Dto;
using EMS.Application.Utility;
using EMS.Domain.DomainEntities.Base;
using EMS.EntityFrameworkCore.EntityFrameworkCore;
using Mbp.AspNetCore.Http.Context;
using Mbp.AspNetCore.Mvc.Convention;
using Mbp.Core.Core;
using Mbp.Core.Modularity;
using Mbp.Ddd.Application.Mbp.UI;
using Mbp.Ddd.Application.System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.FileServer
{
    //[Authorize]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class AttachmentAppService : IAttachmentAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        private readonly IHostingEnvironment _hostingEnvironment = null;

        private readonly HttpUserContext _currentUser = null;

        public AttachmentAppService(DefaultDbContext defaultDbContext, IHostingEnvironment hostingEnvironment, HttpUserContext currentUser)
        {
            _defaultDbContext = defaultDbContext;
            _hostingEnvironment = hostingEnvironment;
            _currentUser = currentUser;
        }

        [HttpPost("UpLoadFile")]
        public virtual async Task<int> UpLoadFile(IFormFile file, [FromForm] AttachmentInputDto attachmentDto)
        {
            // todo 对文件做安全检查

            if (file == null)
            {
                throw new ArgumentNullException("没有接收到文件!");
            }

            if (attachmentDto == null)
            {
                throw new ArgumentNullException("业务参数未定义!");
            }

            var fileDirectory = string.Empty;

            // 文件目录
            fileDirectory = Path.Combine(_hostingEnvironment.ContentRootPath, @"FileRoot\Attach\", attachmentDto.BussinessTypeElementCode, attachmentDto.BussinessId.ToString());

            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            var filePath = Path.Combine(fileDirectory, file.FileName);

            using (var fs = File.Create(filePath))
            {
                await file.CopyToAsync(fs);
            }

            // 写入数据库
            var attachment = _mapper.Map<EmsAttachment>(attachmentDto);
            attachment.Name = file.FileName;
            attachment.Url = $"Attach/{attachmentDto.BussinessTypeElementCode}/{attachmentDto.BussinessId}/{file.FileName}";
            _defaultDbContext.EmsAttachments.Add(attachment);

            return await _defaultDbContext.SaveChangesAsync();
        }

        [HttpGet("GetAttachments")]
        public virtual async Task<PagedList<AttachmentOutputDto>> GetAttachments(Guid bussinessId)
        {
            int total = 0;

            var notices = _defaultDbContext.EmsAttachments.PageByAscending(10, 1, out total,
                (c) => c.BussinessId == bussinessId, (c => c.Id)).ToList();

            // 返回列表分页数据
            return new PagedList<AttachmentOutputDto>()
            {
                Content = _mapper.Map<List<AttachmentOutputDto>>(notices),
                PageIndex = 1,
                PageSize = 10,
                Total = total
            };
        }

        [HttpGet("GetAttachment")]
        public virtual async Task<AttachmentOutputDto> GetAttachment(Guid bussinessId)
        {
            int total = 0;

            var attachment = _defaultDbContext.EmsAttachments.PageByAscending(100, 1, out total,
                (c) => c.BussinessId == bussinessId, (c => c.Id)).FirstOrDefault();

            return _mapper.Map<AttachmentOutputDto>(attachment);
        }

        [HttpPut("ClearUserTemp")]
        public virtual int ClearUserTemp(AttachmentInputDto attachmentDto)
        {
            var fileDirectory = Path.Combine(_hostingEnvironment.ContentRootPath, @"FileRoot\Attach\UserTemp\", attachmentDto.BussinessTypeElementCode, _currentUser.LoginName);

            AttachmentHelper.ClearUserTemp(fileDirectory);

            return 1;
        }

        [HttpGet("FetchAttachment")]
        public async Task<FileResult> FetchAttachment(string fileName, string url)
        {
            var attachmentPath = Path.Combine(_hostingEnvironment.ContentRootPath, "FileRoot", url);
            var extension = Path.GetExtension(url);
            var stream = System.IO.File.OpenRead(attachmentPath);
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[extension];
            return new FileStreamResult(stream, memi) { FileDownloadName = fileName };
        }
    }
}
