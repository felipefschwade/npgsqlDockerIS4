using IdentityServer4.Models;
using System.Collections.Generic;

namespace FarmIO.IS4Auth.IS4Config
{
    public class ApiResources
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "api definition")
                {
                    ApiSecrets = { new Secret("supersecretpass".Sha256()) }
                }
            };
        }
    }
}
