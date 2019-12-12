using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Medical.Ai.Mbdp.Application
{
    /// <summary>
    /// 应用层公共类
    /// </summary>
    public static class ApplicationHelper
    {
        public static string EncryptPwdMd5(string password)
        {
            var config = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables()
                   .Build();
            var md5Salt = config.GetSection("Md5Salt").Value;
            byte[] result;
            // 密码加密 加密方式密文db password:md5(md5(password)+salt),需要前端对密码做一次md5加密传过来
            using (var md5 = MD5.Create())
            {
                result = md5.ComputeHash(Encoding.UTF8.GetBytes(password + md5Salt));
            }

            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }
    }
}
