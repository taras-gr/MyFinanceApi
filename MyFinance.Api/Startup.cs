using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFinance.Domain.Interfaces.Repositories;
using MyFinance.Repositories;
using MyFinance.Repositories.Repositories;
using MyFinance.Services;
using MyFinance.Services.Interfaces;

namespace MyFinance.Api
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

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var jwtSettings = Configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettings);

            var mongoDbSettings = Configuration.GetSection("MongoDbSettings");
            services.Configure<MongoDbSettings>(mongoDbSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
