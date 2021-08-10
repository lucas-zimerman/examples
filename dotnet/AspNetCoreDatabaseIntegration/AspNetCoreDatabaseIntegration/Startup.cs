using AspNetCoreDatabaseIntegration.DataAccess;
using AspNetCoreDatabaseIntegration.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Sentry.AspNetCore;
using System.Collections.Generic;
using System.IO;

namespace AspNetCoreDatabaseIntegration
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
            services.AddTransient<IDapperExceptionTypeRepository, DapperExceptionTypeRepository>();
            services.AddTransient<IEFExceptionTypeRepository, EFExceptionTypeRepository>();
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ExceptionTypeDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Transient);
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                Version = "v1",
                    Title = "Sentry - database sample",
                    Description = @"This API will demonstrate the use-cases of the new Sentry integration with SQL Client and Entity Framework Core.
Return outputs will be limited to 500 items to avoid slowdowns when showing the serialized data on the web."
                });
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "AspNetCoreDatabaseIntegration.xml");
                c.IncludeXmlComments(filePath);
                c.DocInclusionPredicate((_, api) => !string.IsNullOrWhiteSpace(api.GroupName));                
                c.TagActionsBy(api => new List<string>() { api.GroupName });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSentryTracing();

            var serviceProvider = app.ApplicationServices;

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SentrySample.WebApi");
                c.RoutePrefix = "";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ExceptionTypeDbContext>();
                // If LocalDB is not supported, we recommend trying the Docker setup of SQL Server. More information can be found at Readme.md
                context.Database.Migrate();
                ExceptionTypeDbContext.Seed(context);

            }
        }
    }
}
