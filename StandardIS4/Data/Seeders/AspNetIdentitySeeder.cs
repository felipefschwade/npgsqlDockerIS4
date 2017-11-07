using MeuClima.IS4Auth.Data;
using MeuClima.IS4Auth.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FarmIO.IS4Auth.Data.Seeders
{
    public class AspNetIdentitySeeder
    {
        public async static Task InitializeUsers(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var manager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                context.Database.Migrate();
                if (!context.Users.Any())
                {
                    var user = new ApplicationUser
                    {
                        UserName = "root",
                        Email = "gpsati@faculdadelasalle.edu.br",
                        Id = Guid.NewGuid().ToString(),
                        EmailConfirmed = true
                    };
                    await manager.CreateAsync(user, "Lasalle2016&");
                    await manager.AddClaimAsync(user, new Claim("ReadReagentesPermission", "True"));
                }
                context.SaveChanges();
            }

        }
    }
}
