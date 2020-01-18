using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace MvcClient
{
    public class MSALAppMemoryTokenCache
    {
        /// <summary>
        /// The application cache key
        /// </summary>
        internal readonly string AppCacheId;

        /// <summary>
        /// The backing MemoryCache instance
        /// </summary>
        internal readonly MemoryCache memoryCache = MemoryCache.Default;

        /// <summary>
        /// The duration utill the tokens are kept in memory cache. In production, a higher value up to 90 days is recommended.
        /// The token cache will contain both AccessToken and RefreshToken, which they last 1h and 90 days, respectively, by default.
        /// </summary>
        private readonly DateTimeOffset cacheDuration = DateTimeOffset.Now.AddHours(48);

        /// <summary>
        /// Initializes a new instance of the <see cref="MSALAppMemoryTokenCache"/> class.
        /// </summary>
        /// <param name="tokenCache">The client's instance of the token cache.</param>
        /// <param name="clientId">The application's id (Client ID).</param>
        public MSALAppMemoryTokenCache(TokenCache tokenCache, string clientId)
        {
            AppCacheId = clientId + "_AppTokenCache";

            tokenCache.SetBeforeAccess(AppTokenCacheBeforeAccessNotification);
            tokenCache.SetAfterAccess(AppTokenCacheAfterAccessNotification);
            tokenCache.SetBeforeWrite(AppTokenCacheBeforeWriteNotification);
        }

        /// <summary>
        /// if you want to ensure that no concurrent write take place, use this notification to place a lock on the entry
        /// </summary>
        /// <param name="args">Contains parameters used by the MSAL call accessing the cache.</param>
        private void AppTokenCacheBeforeWriteNotification(TokenCacheNotificationArgs args)
        {
            // Since we are using a MemoryCache ,whose methods are threads safe, we need not to do anything in this handler.
        }

        public void Clear()
        {
            memoryCache.Remove(AppCacheId);
        }

        /// <summary>
        /// Triggered right before MSAL needs to access the cache. Reload the cache from the persistence store in case it changed since the last access.
        /// </summary>
        /// <param name="args">Contains parameters used by the MSAL call accessing the cache.</param>
        private void AppTokenCacheBeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            // Load the token cache from memory
            byte[] tokenCacheBytes = (byte[])this.memoryCache.Get(AppCacheId);
            args.TokenCache.Deserialize(tokenCacheBytes); //shouldClearExistingCache: true);
        }

        /// <summary>
        /// Triggered right after MSAL accessed the cache.
        /// </summary>
        /// <param name="args">Contains parameters used by the MSAL call accessing the cache.</param>
        private void AppTokenCacheAfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // if the access operation resulted in a cache update
            if (args.HasStateChanged)
            {
                // Reflect changes in the persistence store
                this.memoryCache.Set(AppCacheId, args.TokenCache.Serialize(), cacheDuration);

            }
        }
    }
}