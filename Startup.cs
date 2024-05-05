using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using server.Services;

namespace server
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
            // Register services
            services.AddScoped<ICardService, CardService>();
            services.AddScoped<IBankService, BankService>();

            // Add controllers
            services.AddControllers();

            // Add SPA static files
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "client\\client\\build";
            });

            // Add HttpContextAccessor
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Use static files
            app.UseStaticFiles();

            // Use SPA static files
            app.UseSpaStaticFiles();

            // Configure SPA
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "client\\client";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            // Use developer exception page in development mode
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Use HTTPS redirection
            app.UseHttpsRedirection();

            // Use routing
            app.UseRouting();

            // Use authorization
            app.UseAuthorization();

            // Configure endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
