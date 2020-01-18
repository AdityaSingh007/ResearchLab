using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Web;

namespace MvcClient
{
    public class MSALPerUserMemoryTokenCache
    {
        /// <summary>
        /// The backing MemoryCache instance
        /// </summary>
        internal readonly MemoryCache memoryCache = MemoryCache.Default;

        /// <summary>
        /// The duration till the tokens are kept in memory cache. In production, a higher value, upto 90 days is recommended.
        /// </summary>
        private readonly DateTimeOffset cacheDuration = DateTimeOffset.Now.AddHours(48);

        /// <summary>
        /// Once the user signes in, this will not be null and can be obtained via a call to Thread.CurrentPrincipal
        /// </summary>
        internal ClaimsPrincipal SignedInUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSALPerUserMemoryTokenCache"/> class.
        /// </summary>
        /// <param name="tokenCache">The client's instance of the token cache.</param>
        public MSALPerUserMemoryTokenCache(TokenCache tokenCache)
        {
            Initialize(tokenCache, ClaimsPrincipal.Current);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MSALPerUserMemoryTokenCache"/> class.
        /// </summary>
        /// <param name="tokenCache">The client's instance of the token cache.</param>
        /// <param name="user">The signed-in user for whom the cache needs to be established.</param>
        public MSALPerUserMemoryTokenCache(TokenCache tokenCache, ClaimsPrincipal user)
        {
            Initialize(tokenCache, user);
        }

        /// <summary>Initializes the cache instance</summary>
        /// <param name="tokenCache">The ITokenCache passed through the constructor</param>
        /// <param name="user">The signed-in user for whom the cache needs to be established..</param>
        private void Initialize(TokenCache tokenCache, ClaimsPrincipal user)
        {
            SignedInUser = user;

            tokenCache.SetBeforeAccess(UserTokenCacheBeforeAccessNotification);
            tokenCache.SetAfterAccess(UserTokenCacheAfterAccessNotification);
            tokenCache.SetBeforeWrite(UserTokenCacheBeforeWriteNotification);

            if (SignedInUser == null)
            {
                // No users signed in yet, so we return
                return;
            }
        }

        /// <summary>
        /// Explores the Claims of a signed-in user (if available) to populate the unique Id of this cache's instance.
        /// </summary>
        /// <returns>The signed in user's object.tenant Id , if available in the ClaimsPrincipal.Current instance</returns>
        internal string GetMsalAccountId()
        {
            if (SignedInUser != null)
            {
                return SignedInUser.GetMsalAccountId();
            }
            return null;
        }

        /// <summary>
        /// Clears the TokenCache's copy of this user's cache.
        /// </summary>
        public void Clear()
        {
            memoryCache.Remove(GetMsalAccountId());
        }

        /// <summary>
        /// Triggered right after MSAL accessed the cache.
        /// </summary>
        /// <param name="args">Contains parameters used by the MSAL call accessing the cache.</param>
        private void UserTokenCacheAfterAccessNotification(TokenCacheNotificationArgs args)
        {
            SetSignedInUserFromNotificationArgs(args);

            // if the access operation resulted in a cache update
            if (args.HasStateChanged)
            {
                string cacheKey = GetMsalAccountId();

                if (string.IsNullOrWhiteSpace(cacheKey))
                    return;

                // Ideally, methods that load and persist should be thread safe.MemoryCache.Get() is thread safe.
                //this.memoryCache.Set(cacheKey, args.TokenCache.SerializeMsalV3(), cacheDuration);
                this.memoryCache.Set(cacheKey, args.TokenCache.Serialize(), cacheDuration);
            }
        }

        /// <summary>
        /// Triggered right before MSAL needs to access the cache. Reload the cache from the persistence store in case it changed since the last access.
        /// </summary>
        /// <param name="args">Contains parameters used by the MSAL call accessing the cache.</param>
        private void UserTokenCacheBeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            string cacheKey = GetMsalAccountId();

            if (string.IsNullOrWhiteSpace(cacheKey))
                return;

            byte[] tokenCacheBytes = (byte[])this.memoryCache.Get(cacheKey);
            //args.TokenCache.DeserializeMsalV3(tokenCacheBytes, shouldClearExistingCache: true);
            args.TokenCache.Deserialize(tokenCacheBytes);
        }

        /// <summary>
        /// if you want to ensure that no concurrent write take place, use this notification to place a lock on the entry
        /// </summary>
        /// <param name="args">Contains parameters used by the MSAL call accessing the cache.</param>
        private void UserTokenCacheBeforeWriteNotification(TokenCacheNotificationArgs args)
        {
            // Since we are using a MemoryCache ,whose methods are threads safe, we need not to do anything in this handler.
        }

        /// <summary>
        /// To keep the cache, ClaimsPrincipal and Sql in sync, we ensure that the user's object Id we obtained by MSAL after
        /// successful sign-in is set as the key for the cache.
        /// </summary>
        /// <param name="args">Contains parameters used by the MSAL call accessing the cache.</param>
        private void SetSignedInUserFromNotificationArgs(TokenCacheNotificationArgs args)
        {
            if (SignedInUser == null && args.Account != null)
            {
                SignedInUser = args.Account.ToClaimsPrincipal();
            }
        }
    }
}