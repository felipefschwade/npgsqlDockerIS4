using FarmIO.IS4Auth.IS4Config;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using MeuClima.IS4Auth.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FarmIO.IS4Auth.Data.Seeders
{
    public class IdentityServerSeeder
    {
        public static void InitializeIdentityResourcesDB(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                CadastraClientes(context);
                CadastraRecursos(serviceScope, context);
                CadastraRecursosDeApi(context);
            }
        }

        private static void CadastraRecursosDeApi(ConfigurationDbContext context)
        {
            if (!context.ApiResources.Any())
            {
                foreach (var resource in ApiResources.GetApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }

        private static void CadastraRecursos(IServiceScope serviceScope, ConfigurationDbContext context)
        {
            var temRecursos = context.IdentityResources.ToList();
            if (!context.IdentityResources.Any())
            {
                foreach (var resource in IS4Resources.GetIdentityResources(serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>()))
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }

        private static void CadastraClientes(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Clients.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}
