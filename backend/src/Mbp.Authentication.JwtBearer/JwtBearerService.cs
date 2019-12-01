using Mbp.Core.Core.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mbp.Authentication.JwtBearer
{
    /// <summary>
    /// 实现Jwt相关动作
    /// </summary>
    public class JwtBearerService : IJwtBearerService
    {
        // TODO 考虑重新创建token之后,使得原先的token过期,避免安全问题

        private readonly IOptions<MbpConfig> _mbpConfigs;

        public JwtBearerService(IOptions<MbpConfig> options)
        {
            _mbpConfigs = options;
        }

        public Task<Jwt> CreateJwt(string userId, string userName, List<Claim> claims)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));

            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));

            claims.Add(new Claim(ClaimTypes.Name, userName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            
            claims.AddRange(claims);

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_mbpConfigs.Value.Jwt.SecurityKey));
            var expires = DateTime.Now.AddMinutes(_mbpConfigs.Value.Jwt.TimeOut);

            var token = new JwtSecurityToken(
                        issuer: _mbpConfigs.Value.Jwt.Issuer,
                        audience: _mbpConfigs.Value.Jwt.Audience,
                        claims: claims,
                        notBefore: DateTime.Now,
                        expires: expires,
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            //生成Token
            return Task.Run(() =>
            {
                return new Jwt()
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(token)
                };
            });
        }
    }
}
