using Mbp.Core.Core.Config;
using Mbp.EntityFrameworkCore.PermissionModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mbp.Authentication.JwtBearer
{
    /// <summary>
    /// 实现Jwt相关动作
    /// </summary>
    public class JwtBearerService : IJwtBearerService
    {
        // TODO 考虑重新创建token之后,使得原先的token过期,避免安全问题

        private readonly IOptions<MbpConfig> _mbpConfigs;

        private readonly IServiceProvider _provider;

        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        public JwtBearerService(IOptions<MbpConfig> options, IServiceProvider provider)
        {
            _mbpConfigs = options;
            _provider = provider;
        }

        public Task<Jwt> CreateJwt(string userId, string userName, List<Claim> claims)
        {
            if (string.IsNullOrEmpty(userId)) throw new ArgumentNullException(nameof(userId));

            if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException(nameof(userName));

            claims.Add(new Claim(ClaimTypes.Name, userName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));

            //生成Token
            return Task.Run(() =>
            {
                return new Jwt()
                {
                    AccessToken = CreateToken(claims, EnumTokenType.AccessToken),
                    RefreshToken = CreateToken(claims, EnumTokenType.RefreshToken)
                };
            });
        }

        // 生成Token通用
        private string CreateToken(List<Claim> claims, EnumTokenType tokenType)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_mbpConfigs.Value.Jwt.SecurityKey));
            var expires = tokenType == EnumTokenType.AccessToken ? DateTime.Now.AddMinutes(_mbpConfigs.Value.Jwt.TimeOut)
                : DateTime.Now.AddMinutes(_mbpConfigs.Value.Jwt.RefreshTimeOut);

            var token = new JwtSecurityToken(
                        issuer: _mbpConfigs.Value.Jwt.Issuer,
                        audience: tokenType == EnumTokenType.AccessToken ? _mbpConfigs.Value.Jwt.Audience : "refreshToken",
                        claims: claims,
                        notBefore: DateTime.Now,
                        expires: expires,
                        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // 验证token是否过期
        private bool IsVerficateAccessToken(string accessToken)
        {
            JwtSecurityToken jwtSecurityToken = _tokenHandler.ReadJwtToken(accessToken);

            return jwtSecurityToken.ValidTo <= DateTime.Now;
        }

        // 从令牌中取出身份信息
        private ClaimsPrincipal GetPrincipalFromAccessToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                return handler.ValidateToken(accessToken, new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_mbpConfigs.Value.Jwt.SecurityKey)),
                    ValidateLifetime = false
                }, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                return null;
            }
        }

        
        public Task<Jwt> RefreshJwt(string refreshJwt)
        {
            // 检查入参
            if (string.IsNullOrEmpty(refreshJwt)) throw new ArgumentNullException(nameof(refreshJwt));

            // 判断刷新令牌的正确性
            JwtSecurityToken jwtSecurityToken = _tokenHandler.ReadJwtToken(refreshJwt);
            string clientId = jwtSecurityToken.Claims.FirstOrDefault(m => m.Type == ClaimTypes.Name)?.Value;
            if (clientId == null)
            {
                throw new Exception("refreshJwt无效: refreshJwt 中不包含 ClaimTypes.Name 声明");
            }
            string userId = jwtSecurityToken.Claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new Exception("refreshJwt无效: RefreshToken 中不包含 ClaimTypes.NameIdentifier 声明");
            }

            // 检查刷新令牌的可用性,不可用则返回空的jwt,使其无法刷新状态

            if (!IsVerficateAccessToken(refreshJwt) || GetPrincipalFromAccessToken(refreshJwt) == null)
            {
                return Task.Run(() =>
                {
                    return new Jwt();
                });
            }

            // todo 判断是否是管理员权限,如果是管理员,重新给管理员令牌

            // 颁发新的令牌
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, clientId));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_mbpConfigs.Value.Jwt.SecurityKey));
            var expires = DateTime.Now.AddMinutes(_mbpConfigs.Value.Jwt.TimeOut);

            return Task.Run(() =>
            {
                return new Jwt()
                {
                    AccessToken = CreateToken(claims, EnumTokenType.AccessToken),
                    RefreshToken = CreateToken(claims, EnumTokenType.RefreshToken)
                };
            });
        }
    }
}
