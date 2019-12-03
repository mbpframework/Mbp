using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mbp.Authentication.JwtBearer
{
    /// <summary>
    /// 提供Jwt相关的一些操作
    /// </summary>
    public interface IJwtBearerService
    {
        /// <summary>
        /// 生成Jwt
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        Task<Jwt> CreateJwt(string userId, string userName, List<Claim> claims);

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshJwt"></param>
        /// <returns></returns>
        Task<Jwt> RefreshJwt(string refreshJwt);
    }
}
