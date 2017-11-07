using IdentityModel;
using IdentityServer4.Models;
using MeuClima.IS4Auth.Data;
using System.Collections.Generic;
using System.Linq;

namespace FarmIO.IS4Auth.IS4Config
{
    public class IS4Resources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources(ApplicationDbContext context)
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "UserPermissions",
                    DisplayName = "All User Permissions",
                    UserClaims = GetClaims(context)
                }
            };
        }
        private static ICollection<string> GetClaims(ApplicationDbContext context)
        {
            var claims = context.UserClaims.Where(c => c.ClaimType.Contains("Permission")).ToList();
            ICollection<string> NameClaimList = new List<string>();
            foreach (var claim in claims)
            {
                NameClaimList.Add(claim.ClaimType);
            }
            return NameClaimList;
        }
    }
}
