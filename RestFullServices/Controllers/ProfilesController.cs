﻿using RestFullServices.Helpers;
using SocialNetwork.Data.Models;
using SocialNetwork.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace RestFullServices.Controllers
{
    public class ProfilesController : SocialNetworkApiController
    {
        private readonly IProfileRepository profileRepository;
        private readonly IUserRepository userRepository;

        public ProfilesController(IProfileRepository profileRepository, IUserRepository userRepository)
        {
            this.profileRepository = profileRepository;
            this.userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAsync(string username, string password)
        {
            var user = await userRepository.GetAsync(username, HashHelper.Sha512(password + username));

            if (user == null)
            {
                return NotFound();
            }

            var profile = await profileRepository.GetForAsync(user);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpGet]
        [ScopeAuthorize("Read")]
        public async Task<IHttpActionResult> GetAsync()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            var username = claimsPrincipal?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            var user = await userRepository.GetAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            var profile = await profileRepository.GetForAsync(user);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPut]
        public async Task<IHttpActionResult> PutAsync(string username, string password, [FromBody]Profile profile)
        {
            var user = await userRepository.GetAsync(username, HashHelper.Sha512(password + username));

            if (user == null)
            {
                return NotFound();
            }

            var toUpdate = await profileRepository.GetForAsync(user);

            if (toUpdate == null)
            {
                return NotFound();
            }

            toUpdate.Bio = profile.Bio;

            await profileRepository.UpdateAsync(toUpdate);

            return Ok(toUpdate);
        }
    }
}
