using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace OAuthServer
{
    public class InMemoryManager
    {
        public List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>()
            {
                new InMemoryUser()
                {
                    Subject="adityasingh5@kpmg.com",
                    Username="adityasingh5@kpmg.com",
                    Password="Aditya123",
                    Claims=new []
                    {
                        new Claim(Constants.ClaimTypes.Name,"Aditya Singh")
                    }
                },
                new InMemoryUser()
                {
                    Subject="mail@filipekberg.se",
                    Username="mail@filipekberg.se",
                    Password="password",
                    Claims=new []
                    {
                        new Claim(Constants.ClaimTypes.Name,"filip ekberg"),
                        new Claim(Constants.ClaimTypes.Role,"Admin")
                    }
                }
            };
        }

        public IEnumerable<Scope> GetScopes()
        {
            return new[]
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.OfflineAccess,
                new Scope()
                {
                    Name="Read",
                    DisplayName="Read User Data"
                },
                new Scope()
                {
                    Name="Role",
                    DisplayName="Your Role(s)"
                }
            };
        }

        public IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client()
                {
                    ClientId="camps",
                    ClientSecrets=new List<Secret>()
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientName="SocialNetwork",
                    Flow=Flows.ResourceOwner,
                    AllowedScopes=new List<string>()
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.OfflineAccess,
                        "Read"
                    },
                    Enabled=true
                },
                new Client()
                {
                    ClientId="socialnetwork_implicit",
                    ClientSecrets=new List<Secret>()
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientName="SocialNetwork",
                    Flow=Flows.Implicit,
                    AllowedScopes=new List<string>()
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.OfflineAccess,
                        "Read"
                    },
                    RedirectUris=new List<string>()
                    {
                        "http://localhost:41109"
                    },
                    PostLogoutRedirectUris=new List<string>(){
                        "http://localhost:41109"
                    },
                    Enabled=true
                },
                new Client()
                {
                    ClientId="scoialnetwork_code",
                    ClientSecrets=new List<Secret>()
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientName="SocialNetwork",
                    Flow=Flows.Hybrid,
                    AllowedScopes=new List<string>()
                    {
                        Constants.StandardScopes.OpenId,
                        Constants.StandardScopes.Profile,
                        Constants.StandardScopes.OfflineAccess,
                        "Read",
                        "Role"
                    },
                    RedirectUris=new List<string>()
                    {
                        "http://localhost:41109/"
                    },
                    PostLogoutRedirectUris=new List<string>(){
                        "http://localhost:41109"
                    },
                    Enabled=true
                }

            };
        }
    }
}