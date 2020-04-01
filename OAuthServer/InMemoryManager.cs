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
                },
                 new InMemoryUser
            {
                Username = "Kevin",
                Password = "secret",
                Subject = "1",

                Claims = new[]
                {
                    new Claim(Constants.ClaimTypes.GivenName, "Kevin"),
                    new Claim(Constants.ClaimTypes.FamilyName, "Dockx"),
                    new Claim(Constants.ClaimTypes.Role, "WebReadUser"),
                    new Claim(Constants.ClaimTypes.Role, "WebWriteUser"),
                    new Claim(Constants.ClaimTypes.Role, "MobileReadUser"),
                    new Claim(Constants.ClaimTypes.Role, "MobileWriteUser")
                }
            }
            ,
            new InMemoryUser
            {
                Username = "Sven",
                Password = "secret",
                Subject = "2",

                Claims = new[]
                {
                    new Claim(Constants.ClaimTypes.GivenName, "Sven"),
                    new Claim(Constants.ClaimTypes.FamilyName, "Vercauteren"),
                    new Claim(Constants.ClaimTypes.Role, "WebReadUser"),
                    new Claim(Constants.ClaimTypes.Role, "MobileReadUser")
                }
            },

            new InMemoryUser
            {
                Username = "Nils",
                Password = "secret",
                Subject = "3",

                Claims = new[]
                {
                    new Claim(Constants.ClaimTypes.GivenName, "Nils"),
                    new Claim(Constants.ClaimTypes.FamilyName, "Missorten"),
                    new Claim(Constants.ClaimTypes.Role, "WebWriteUser"),
                    new Claim(Constants.ClaimTypes.Role, "MobileWriteUser")
                }
            },

            new InMemoryUser
            {
                Username = "Kenneth",
                Password = "secret",
                Subject = "4",

                Claims = new[]
                {
                    new Claim(Constants.ClaimTypes.GivenName, "Kenneth"),
                    new Claim(Constants.ClaimTypes.FamilyName, "Mills"),
                    new Claim(Constants.ClaimTypes.Role, "WebReadUser"),
                    new Claim(Constants.ClaimTypes.Role, "WebWriteUser")
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
                },
                 new Scope
                    {
                        Name = "expensetrackerapi",
                        DisplayName = "ExpenseTracker API Scope",
                        Type = ScopeType.Resource,
                        Emphasize = false,
                         Enabled = true,
                          Claims = new List<ScopeClaim>
                        {
                            new ScopeClaim("role")
                        }

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
                },
                new Client
                {
                     Enabled = true,
                     ClientId = "expensetracker",
                     ClientName = "ExpenseTracker MVC Client (Auth Code Flow)",
                     Flow = Flows.AuthorizationCode,
                     RequireConsent = true,  
      
                    // redirect = URI of MVC app
                    RedirectUris = new List<string>
                    {
                         "http://localhost:41109/"
                    },

                    ClientSecrets = new List<Secret>()
                    {
                        new Secret("secret".Sha256())
                    }

                 },
                new Client
                {
                  ClientId = "postmantestclient",
                  ClientName = "Postman http test client",
                  Flow = Flows.AuthorizationCode,
                  AllowAccessToAllScopes = true,
			      RequireConsent = false,
                  
                  ClientSecrets = new List<Secret>
                  {
                      new Secret("PostmanSecret".Sha256())
                  },
                  
                  RedirectUris = new List<string>()
                  {
                      "https://www.getpostman.com/oauth2/callback"
                  }
                }

            };
        }
    }
}