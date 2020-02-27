using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Couchbase.Extensions.DependencyInjection;
using HollywoodBetsGamingCenter.Api.Interfaces;
using HollywoodBetsGamingCenter.Api.Models;
using HollywoodBetsGamingCenter.Api.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HollywoodBetsGamingCenter.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            string ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string COUCHBASE_SERVER = Environment.GetEnvironmentVariable("COUCHBASE_SERVER");
            string COUCHBASE_USERNAME = Environment.GetEnvironmentVariable("COUCHBASE_USERNAME");
            string COUCHBASE_PASSWORD = Environment.GetEnvironmentVariable("COUCHBASE_PASSWORD");

            if (ASPNETCORE_ENVIRONMENT.ToLower() == "development")
                services.AddCouchbase(Configuration.GetSection("Couchbase"));
            else
            {
                services.AddCouchbase(client =>
                {
                    client.Servers = new List<Uri> { new Uri(COUCHBASE_SERVER) };
                    client.Username = COUCHBASE_USERNAME;
                    client.Password = COUCHBASE_PASSWORD;
                    client.UseSsl = false;
                });
            }

            services.AddScoped<IRepository<Game>, GameRepository>();
            services.AddScoped<IRepository<Prize>, PrizeRepository>();
            services.AddScoped<IRepository<Transaction>, TransactionRepository>();
            services.AddScoped<IRepository<ResultModel>, ResultRepository>();
            services.AddScoped<IRepository<Log>, LogRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "HollywoodBets Gaming Center", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HollywoodBets Gaming Center");
            });
        }
    }
}
