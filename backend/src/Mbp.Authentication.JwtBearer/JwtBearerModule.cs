using Autofac;
using Castle.Core.Configuration;
using Mbp.AspNetCore.Http.Context;
using Mbp.Core.Core.Config;
using Mbp.Core.Modularity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mbp.Authentication.JwtBearer
{
    public class JwtBearerModule : AspNetCoreModule
    {
        public override EnumModuleGrade Level => EnumModuleGrade.Component;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var jwtConfig = serviceProvider.GetService<IOptions<MbpConfig>>().Value?.Jwt;

            services.TryAddScoped<IJwtBearerService, JwtBearerService>();

            //配置授权
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(jwtBearerOptions =>
            {
                // 设置验证参数
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtConfig.SecurityKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(jwtConfig.TimeOut)
                };
                // 设置验证事件
                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("action", "timeOut");
                        }
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        // todo 解析token自动续费时间
                        var userContext = context.HttpContext.RequestServices.GetService<HttpUserContext>();
                        var claims = context.Principal.Claims;
                        userContext.Id = int.Parse(claims.First(x => x.Type == ClaimTypes.Sid).Value);
                        userContext.LoginName = claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
                        userContext.UserName = claims.First(x => x.Type == ClaimTypes.Name).Value;
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddScoped<HttpUserContext>();

            return base.AddServices(services);
        }

        public override void UseModule(IApplicationBuilder app)
        {
            // 鉴权中间件
            app.UseAuthentication();
            base.UseModule(app);
        }
    }
}
