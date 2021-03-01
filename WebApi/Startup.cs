using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApi.Convertrs;
using WebApi.Core;
using WebApi.Core.Exceptions.ExceptionLogger;
using WebApi.Core.Models;
using WebApi.Core.Repositories;
using WebApi.Core.Services.Articulos;
using WebApi.Core.Services.ClientApplications;
using WebApi.Filters;
using WebApi.Helpers;
using WebApi.Infrastructure;
using WebApi.Infrastructure.Repositories;
using WebApi.Middleware;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _environment = env;
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            ConfigureAutomapper(services);
            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            //ConfigureSecurity(services, appSettings);

            ConfigureDatabase(services, Configuration.GetConnectionString("WebApiDatabase"));


            ConfigureSwagger(services);
            string[] allowedCors = Configuration.GetSection("CorsAllowed").Get<string[]>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                                  builder =>
                                  {
                                      builder.WithOrigins(allowedCors)
                                      .AllowCredentials()
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });

            ConfigureServicesAndRepositories(services);
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.


            app.ConfigureExceptionHandler(env, serviceProvider.GetService<IExceptionLogger>());
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureAutomapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile)).
                AddAutoMapper(typeof(CoreMappingProfile)).
                AddAutoMapper(typeof(InfrastructureMappingProfile));
        }

        public static void ConfigureServicesAndRepositories(IServiceCollection services)
        {
            //Services
            services.AddScoped<IArticuloService, ArticuloService>();


           // services.AddSingleton<IExceptionLogger, ExceptionLogger>();


            //Repositories
            services.AddScoped(typeof(IWebApiRepository<>), typeof(WebApiRepository<>));
           //s services.AddSingleton(typeof(ILoggerRepository), typeof(LoggerRepository));


        }

        public static void ConfigureDatabase(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<WebApiContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
                });
        }

        public virtual void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.DocumentFilter<LowercaseDocumentFilter>();
            });
        }

        public virtual void ConfigureSecurity(IServiceCollection services, AppSettings appSettings)
        {
            JwtSecurityTokenHelper.Initialize(appSettings);
            var key = Encoding.ASCII.GetBytes(appSettings.JwtSecret);

            if (_environment.IsDevelopment())
            {
                services.AddAuthentication().AddScheme<AuthenticationSchemeOptions, DevelopmentAnonymousAuthenticationHandler>("ClientApplication", null);
            }
            else
            {
                services.AddAuthentication().AddJwtBearer("ClientApplication", x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            return TokenValidationForPolicyClientApplication(context);
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = appSettings.JwtIssuer,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            }


            services.AddAuthentication()
            .AddJwtBearer("All", x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        return TokenValidationForPolicyAll(context);
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = appSettings.JwtIssuer,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });


            services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, BasicApplicationClientAuthenticationHandler>("BasicClientAuthentication", null);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("All", policy =>
                {
                    policy.AuthenticationSchemes.Add("All");
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy("ClientApplication", policy =>
                {
                    policy.AuthenticationSchemes.Add("ClientApplication");
                    policy.RequireAuthenticatedUser();
                });

                options.AddPolicy("BasicClientAuthentication", policy =>
                {
                    policy.AuthenticationSchemes.Add("BasicClientAuthentication");
                    policy.RequireAuthenticatedUser();
                });
            });
        }

        private Task TokenValidationForPolicyClientApplication(TokenValidatedContext context)
        {
            var id = int.Parse(context.Principal.Identity.Name);
            var roleClaim = context.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

            if (roleClaim == null || ((TokenRoleEnum)int.Parse(roleClaim.Value)) != TokenRoleEnum.ClientApplication)
                context.Fail("Unauthorized");

            var clientApplicationService = context.HttpContext.RequestServices.GetRequiredService<IClientApplicationService>();
            var clientApplication = clientApplicationService.GetById(id);

            if (clientApplication == null || !clientApplication.IsActive)
                context.Fail("Unauthorized");

            return Task.CompletedTask;
        }

        private static Task TokenValidationForPolicyAll(TokenValidatedContext context)
        {
            int id = int.Parse(context.Principal.Identity.Name);
            var roleClaim = context.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

            if (roleClaim == null)
                context.Fail("Unauthorized");

            TokenRoleEnum tokenRole = (TokenRoleEnum)int.Parse(roleClaim.Value);

            switch (tokenRole)
            {

                case TokenRoleEnum.ClientApplication:
                    {
                        var clientApplicationService = context.HttpContext.RequestServices.GetRequiredService<IClientApplicationService>();
                        var clientApplication = clientApplicationService.GetById(id);
                        if (clientApplication == null || !clientApplication.IsActive)
                            context.Fail("Unauthorized");
                        break;
                    }
                default:
                    context.Fail("Unauthorized");
                    break;
            }
            return Task.CompletedTask;
        }
    }
}

