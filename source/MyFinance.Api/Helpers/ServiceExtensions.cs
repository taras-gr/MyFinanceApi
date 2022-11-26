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
using MyFinance.Repositories.CosmosDb;
using MyFinance.Repositories.Interfaces;
using MyFinance.Repositories.Repositories;
using MyFinance.Services.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyFinance.Api.Helpers
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DevelopmentPolicy",
                    builder => builder.WithOrigins("http://localhost:4200", "http://3.235.68.38", "https://localhost:7225")
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
            var region = configuration["AWSCognitoSettings:Region"];
            var poolId = configuration["AWSCognitoSettings:UserPoolId"];
            var appClientId = configuration["AWSCognitoSettings:UserPoolClientId"];
            var json = new WebClient().DownloadString($"https://cognito-idp.{region}.amazonaws.com/{poolId}/.well-known/jwks.json");

            services
              .AddAuthentication(options =>
              {
                  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              })
              .AddJwtBearer(options =>
              {
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                      {
                          return JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                      },
                      ValidateIssuer = true,
                      ValidIssuer = $"https://cognito-idp.{region}.amazonaws.com/{poolId}",
                      ValidateLifetime = true,
                      LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                      ValidateAudience = true,
                      ValidAudience = appClientId,
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

        public static void ConfigureAWSCognitoSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var AWSCognitoSettings = configuration.GetSection("AWSCognitoSettings");
            services.Configure<AWSCognitoSettings>(AWSCognitoSettings);
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

        public static async Task ConfigureCosmosDbAsync(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            var databaseName = configurationSection["DatabaseName"];
            var categoriesContainerName = configurationSection["CategoriesContainerName"];
            var expensesContainerName = configurationSection["ExpensesContainerName"];

            var account = configurationSection["Account"];
            var key = configurationSection["Key"];

            var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);

            var categoryRepository = new CosmosDbCategoryRepository(client, databaseName, categoriesContainerName);
            var expenseRepository = new CosmosDbExpenseRepository(client, databaseName, expensesContainerName);

            services.AddSingleton<ICategoryRepository>(categoryRepository);
            services.AddSingleton<IExpenseRepository>(expenseRepository);
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