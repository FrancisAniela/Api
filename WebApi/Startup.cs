using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using WebApi.Convertrs;
using WebApi.Core;
using WebApi.Core.Exceptions.ExceptionLogger;
using WebApi.Core.Models;
using WebApi.Core.Repositories;
using WebApi.Core.Services.Articulos;
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
            // ConfigureServicesAndRepositories(services);

           
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
            });
        }
    }
}

