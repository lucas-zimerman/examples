using AspNetCoreDatabaseIntegration.DataAccess;
using AspNetCoreDatabaseIntegration.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Sentry.AspNetCore;

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
            services.AddTransient<IDapperBugRepository, DapperBugRepository>();
            services.AddTransient<IEFBugRepository, EFBugRepository>();
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<BugEfDbContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Sentry - database sample",
                });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArchitecture.WebApi");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<BugEfDbContext>();
                context.Database.Migrate();
                BugEfDbContext.Seed(context);

            }
        }
    }
}
