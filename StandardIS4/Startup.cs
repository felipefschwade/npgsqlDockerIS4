using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MeuClima.IS4Auth.Data;
using MeuClima.IS4Auth.Models;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Reflection;

namespace MeuClima.IS4Auth
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
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration["ConnectionString"]));

            services = AddIdentityServer(services, migrationsAssembly);

            services.AddCors();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseCors(build =>
            {
                build.AllowAnyHeader();
                build.AllowAnyOrigin();
                build.AllowAnyMethod();
            });

            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private IServiceCollection AddIdentityServer(IServiceCollection services, string migrationsAssembly)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddConfigurationStore(opt =>
                {
                    opt.ConfigureDbContext = builder =>
                        builder.UseNpgsql(Configuration["ConnectionString"], sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(opt =>
                {
                    opt.ConfigureDbContext = builder =>
                        builder.UseNpgsql(Configuration["ConnectionString"], sql => sql.MigrationsAssembly(migrationsAssembly));
                    opt.EnableTokenCleanup = true;
                    opt.TokenCleanupInterval = 30;
                })
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<ApplicationUser>();

            return services;
        }

    }
}
