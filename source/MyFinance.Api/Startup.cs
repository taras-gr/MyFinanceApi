using System;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.CognitoAuthentication;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using MyFinance.Api.Helpers;
using MyFinance.Repositories.DynamoDb;
using MyFinance.Repositories.Interfaces;
using MyFinance.Repositories.Repositories;
using MyFinance.Services;
using MyFinance.Services.Interfaces;

namespace MyFinance.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public string API_PREFIX { get; private set; } = "/Prod";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2C"));

            //services.AddCognitoIdentity();

            services.AddControllers();

            //services.AddDefaultAWSOptions(Configuration.GetAWSOptions());

            //Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", Configuration["AWS:AccessKey"]);
            //Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY", Configuration["AWS:SecretKey"]);
            //Environment.SetEnvironmentVariable("AWS_REGION", Configuration["AWS:Region"]);

            //services.AddAWSService<IAmazonDynamoDB>();
            //services.AddScoped<IDynamoDBContext, DynamoDBContext>();

            //services.AddAWSService<Amazon.S3.IAmazonS3>();

            services.AddScoped<IUserService, UserService>();

            //services.AddScoped<IAWSCognitoService, AWSCognitoService>();

            services.AddScoped<IUserRepository, DynamoDbUserRepository>();

            services.AddScoped<IExpenseService, ExpenseService>();

            services.AddScoped<IExpenseRepository, ExpenseRepository>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IStatisticsService, StatisticsService>();

            services.AddScoped<IAuthenticationManagerService, AuthenticationManagerService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());            

            services.ConfigureSqlDbContext(Configuration, WebHostEnvironment);

            //services.ConfigureJwtSetting(Configuration);

            services.ConfigureMongoDbSettings(Configuration);

            //services.ConfigureAWSCognitoSettings(Configuration);

            //services.ConfigureJwtAuthentication(Configuration);

            services.ConfigureCors();

            services.ConfigureSwagger(WebHostEnvironment);    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (WebHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("DevelopmentPolicy");

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    $"{(WebHostEnvironment.IsDevelopment() ? "" : API_PREFIX)}/swagger/MyFinanceApiOpenAPISecification/swagger.json", 
                    "MyFinanceAPI");
                setupAction.RoutePrefix = "";
            });

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
