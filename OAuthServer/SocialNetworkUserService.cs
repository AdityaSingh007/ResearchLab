using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using SocialNetwork.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OAuthServer
{
    public class SocialNetworkUserService : UserServiceBase
    {
        private readonly IUserRepository userRepository;
        public SocialNetworkUserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public override async Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            //var pwd = HashHelper.Sha512(context.Password + context.UserName);
            var user = await userRepository.GetAsync(context.UserName, HashHelper.Sha512(context.Password + context.UserName));
            if (user == null)
            {
                context.AuthenticateResult = new AuthenticateResult("Incorrect credentials");
                return;
            }

            //context.AuthenticateResult = new AuthenticateResult("/terms",context.UserName, context.Password);
            context.AuthenticateResult = new AuthenticateResult(context.UserName, context.Password);
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = context.Subject.Claims.Select(claim => new Claim(claim.Type, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer, claim.Subject)).ToList();
            var roleClaim = context.Client.Claims.FirstOrDefault();
            claims.Add(new Claim(roleClaim.Type,roleClaim.Value,roleClaim.ValueType,roleClaim.Issuer,roleClaim.OriginalIssuer,roleClaim.Subject));
            context.IssuedClaims = claims;

            return Task.FromResult(0);
        }
    }
}