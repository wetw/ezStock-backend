using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;

namespace ezStock
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var nLogConfigName = "NLog.config";
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (!string.IsNullOrWhiteSpace(env) && File.Exists($"NLog.{env}.config"))
            {
                nLogConfigName = $"NLog.{env}.config";
            }
            NLogBuilder.ConfigureNLog(nLogConfigName).GetCurrentClassLogger();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseNLog()
                .UseStartup<Startup>();
    }
}
