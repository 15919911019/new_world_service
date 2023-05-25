
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CBP.Main.Core.CoreAutoMapper;
using CBP.Main.Core.Service;
using Public.Log;
using System.Collections.Generic;
using System.Linq;
using Public.Tools;
using System;
using System.Threading;
using Public.DbHelper.Mongon;
using Newtonsoft.Json.Linq;
using System.IO;

namespace CBP.Main
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            services.AddDistributedMemoryCache();
            services.AddCors(option =>
            {
                option.AddPolicy("all", policy => policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    //.AllowCredentials()
                    //.WithOrigins(new[] { "http://localhost:8080", "http://42.192.121.66:2021/", "http://localhost:81" })
                    );
            });

            AutoMapperRegister.AutoMapperProfilesRegister(services);
            ServiceRegister.Regiser(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
                app.UseHsts();

            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("all");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("all");
            });
        }
    }

}
