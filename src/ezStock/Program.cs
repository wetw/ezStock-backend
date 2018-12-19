using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.IO;

namespace ezStock
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var nLogConfigName = "NLog.config";
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (!string.IsNullOrWhiteSpace(env) && File.Exists($"NLog.{env}.config"))
            {
                nLogConfigName = $"NLog.{env}.config";
            }
            var logger = NLogBuilder.ConfigureNLog(nLogConfigName).GetCurrentClassLogger();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseNLog()
                .UseStartup<Startup>();
    }
}
