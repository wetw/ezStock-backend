using ezStock.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;
using System.Security.Authentication;
using System.Text;

namespace ezStock
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddMvc(options =>
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
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
                });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
