using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyFinance.Api.Helpers;
using MyFinance.Repositories.Interfaces;
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

            services.AddScoped<IUserRepository, UserRepositoryRdms>();

            services.AddScoped<IExpenseService, ExpenseService>();

            services.AddScoped<IExpenseRepository, ExpenseRepository>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());            

            services.ConfigureSqlDbContext(Configuration);

            services.ConfigureJwtSetting(Configuration);

            services.ConfigureMongoDbSettings(Configuration);

            services.ConfigureJwtAuthentication(Configuration);

            services.ConfigureCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("DevelopmentPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }   
    }
}
