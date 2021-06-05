using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env ,ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsStaging() || env.IsProduction() || env.IsEnvironment("UAT"))
            {

            }
            app.UseFileServer(); // this middleware combins two midllewares(UseStaticFiles() , UseDefaultFiles())
            //app.UseStaticFiles();
            //app.UseDefaultFiles();
            app.Use(async (context, next) =>
            {
                logger.LogInformation("mw1: Incomming request");
                await next();
                logger.LogInformation("mw1: OutComming request");
            });

            app.Use(async (context, next) =>
            {
                logger.LogInformation("mw2: Incomming request");
                await next();
                logger.LogInformation("mw2: OutComming request");
            });

            app.Run(async (context) =>
            {
                throw new Exception("bug");
                logger.LogInformation("final midlleware");
                await context.Response.WriteAsync(_config["Key"]);
            });

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
