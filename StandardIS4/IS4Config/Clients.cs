using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace FarmIO.IS4Auth.IS4Config
{
    public class Clients
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "DefaultClient",
                    ClientName = "A Default Client",
                    ClientSecrets = { new Secret("supersecret".Sha256()) },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OpenId,
                    },
                    AccessTokenLifetime = 1200,
                    AllowOfflineAccess = true
                }
            };
        }
    }
}
