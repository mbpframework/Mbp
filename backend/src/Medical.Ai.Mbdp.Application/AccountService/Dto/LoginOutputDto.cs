using Mbp.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.Application.AccountService.Dto
{
    public class LoginOutputDto
    {
        /// <summary>
        /// 身份凭据
        /// </summary>
        public Jwt AccessToken { get; set; }

        /// <summary>
        /// 可见菜单 todo 设计成通用方案
        /// </summary>
        public List<string> Menus { get; set; } = new List<string>();
    }
}
