using Autofac;
using Castle.Core.Configuration;
using Mbp.Core.Core.Config;
using Mbp.Core.Modularity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
                    }
                };
            });

            return base.AddServices(services);
        }

        public override void UseModule(IApplicationBuilder app)
        {
            app.UseAuthentication();
            base.UseModule(app);
        }
    }
}
