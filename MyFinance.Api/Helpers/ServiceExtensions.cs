using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyFinance.Api.OperationFilters;
using MyFinance.Repositories;
using MyFinance.Repositories.Repositories;
using MyFinance.Services.Helpers;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace MyFinance.Api.Helpers
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DevelopmentPolicy",
                    builder => builder.WithOrigins("http://localhost:4200", "http://50.19.177.139:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination"));
            });
        }

        public static void ConfigureJwtSetting(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSettings);
        }

        public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:JwtSecret"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    
        public static void ConfigureSqlDbContext(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            var loggerFactory = new LoggerFactory();
            var sqlServerConnectionString = configuration.GetConnectionString("SqlServer");

            services.AddDbContext<MyFinanceContext>(options =>
                options
                .UseSqlServer(sqlServerConnectionString)
                .UseLoggerFactory(loggerFactory.GetLoggerFactory())
                .EnableSensitiveDataLogging(env.IsDevelopment())
               );
        }

        public static void ConfigureMongoDbSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbSettings = configuration.GetSection("MongoDbSettings");
            services.Configure<MongoDbSettings>(mongoDbSettings);
        }
    
        public static void ConfigureSwagger(this IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(
                    "MyFinanceApiOpenAPISecification",
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = "MyFinance API",
                        Version = "1",
                        Description = "Through this API you can access categories and expenses.",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                        {
                            Email = "tar.grytsenko@gmail.com",
                            Name = "Tarik Batman",
                            Url = new Uri("https://www.instagram.com/tarasgrytsenko")
                        },
                        License = new Microsoft.OpenApi.Models.OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });

                OpenApiServerProvider openApiServerProvider = new OpenApiServerProvider();

                if (env.IsDevelopment())
                {
                    openApiServerProvider.SetOpenApiServerBuilder(
                        new DevelopmentServerBuilder()
                    );
                }

                if(env.IsProduction())
                {
                    openApiServerProvider.SetOpenApiServerBuilder(
                        new ProductionServerBuilder()
                    );
                }

                openApiServerProvider.ConstructOpenApiServer();

                setupAction.AddServer(openApiServerProvider.GetServer());

                setupAction.OperationFilter<AddAuthHeaderOperationFilter>();
                setupAction.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Description = "`Token only!!!` - without `Bearer_` prefix",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });

                var xmlDocFileName = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                var absolutePathForDocs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + xmlDocFileName);

                setupAction.IncludeXmlComments(absolutePathForDocs);
            });
        }
    }

    public class LoggerFactory
    {
        public ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                   builder.AddConsole()
                          .AddFilter(DbLoggerCategory.Database.Command.Name,
                                     LogLevel.Information));
            return serviceCollection.BuildServiceProvider()
                    .GetService<ILoggerFactory>();
        }
    }
}