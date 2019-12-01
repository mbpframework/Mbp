using Autofac;
using Mbp.Core.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mbp.Swagger
{
    public class SwaggerModule : AspNetCoreModule
    {
        public override EnumModuleGrade Level => EnumModuleGrade.Component;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            // 注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Mbp Auto WebApi", Version = "v1" });

                // TODO:一定要返回true！
                options.DocInclusionPredicate((docName, description) =>
                {
                    return true;
                });
                // TO DO swagger UI加入注释
                Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml").ToList().ForEach(file =>
                {
                    options.IncludeXmlComments(file);
                });
                // Jwt集成
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token,格式:Bearer {Token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { "readAccess", "writeAccess" }
                    }
                });
            });

            return base.AddServices(services);
        }

        public override void UseModule(IApplicationBuilder app)
        {
            IConfiguration configuration = app.ApplicationServices.GetService<IConfiguration>();

            // 启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            // 启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mbp Auto WebApi");
                c.RoutePrefix = string.Empty;
            });

            base.UseModule(app);
        }
    }
}
