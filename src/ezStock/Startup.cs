using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;
using System.Security.Authentication;
using System.Text;
using ezStock.DependencyInjection;
using ezStock.Filters;
using ezStock.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ezStock
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices(_configuration);
            services.AddControllers(options =>
                {
                    options.Filters.Add(new UnhandledExceptionFilterAttribute()
                        .Register<InvalidOperationException>(HttpStatusCode.BadRequest) //400
                        .Register<ArgumentException>(HttpStatusCode.BadRequest)
                        .Register<ArgumentNullException>(HttpStatusCode.BadRequest)
                        .Register<ArgumentOutOfRangeException>(HttpStatusCode.BadRequest)
                        .Register<AuthenticationException>(HttpStatusCode.Unauthorized) //401
                        .Register<UnauthorizedAccessException>(HttpStatusCode.Unauthorized)
                        .Register<InvalidCredentialException>(HttpStatusCode.Unauthorized)
                        .Register<SecurityException>(HttpStatusCode.Forbidden) //403
                        .Register<KeyNotFoundException>(HttpStatusCode.NotFound) //404
                        .Register<FileNotFoundException>(HttpStatusCode.NotFound)
                        .Register<DirectoryNotFoundException>(HttpStatusCode.NotFound)
                        .Register<IndexOutOfRangeException>(HttpStatusCode.NotFound)
                        .Register<TimeoutException>(HttpStatusCode.RequestTimeout) //408
                        .Register<OutOfMemoryException>(HttpStatusCode.RequestEntityTooLarge) //413
                        .Register<InsufficientMemoryException>(HttpStatusCode.RequestEntityTooLarge)
                        .Register<NotSupportedException>(HttpStatusCode.UnsupportedMediaType)); //415
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
                });

            services.AddHealthChecks()
                .AddMemoryHealthCheck("Memory")
                .AddUrlGroup(new Uri("https://www.twse.com.tw/exchangeReport/MI_INDEX"), "TWSE API");
            services.AddHealthChecksUI();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            })
                .UseHealthChecksUI(options => options.UIPath = "/hc-ui");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwagger()
                .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            app.UseRouting()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
