using AspNetCoreRateLimit;
using AuthApp.Extension;
using AuthApp.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApp
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
            services.ConfigureRateLimit();
            services.CORSExtension();            
            services.JwtConfiguration();
            services.CustomServices();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                //this is because the apicontroller attribute automatically do the validation and it cause the request pointer return the request from
                //entering the action filter which is customised for custom validations.
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddControllers(options =>
            {
                //adds a filter as a service globally
                options.Filters.AddService<ModelValidationFilterAttribute>();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthApp", Version = "v1" });
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseIpRateLimiting();
            app.UseClientRateLimiting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthApp v1"));
            }

            //app.UseClientRateLimiting();

            app.CustomLog();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(); 
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
