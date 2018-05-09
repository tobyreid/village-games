using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Village.Games.Auth
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("village-games", "Village Games API"  )
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {

                new Client
                {
                    ClientId = "swagger-ui",
                    ClientName = "Swagger UI Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {
                        "http://localhost:5001/swagger/o2c.html",
                        "http://localhost:5001/swagger/oauth2-redirect.html" ,
                        "https://village-games.azurewebsites.net/api/swagger/oauth2-redirect.html",
                        "https://village-games-api.azurewebsites.net/swagger/oauth2-redirect.html",
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:5001/swagger/",
                        "https://village-games.azurewebsites.net/api/swagger/",
                        "https://village-games.azurewebsites.net/swagger/"
                    },
                    AllowedScopes =
                    {
                        "village-games"
                    },
                    RequireConsent = false
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
        }
    }
}
