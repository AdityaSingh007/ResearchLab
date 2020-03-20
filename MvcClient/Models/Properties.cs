using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcClient.Models
{
    public class Properties
    {
        public readonly string RoleDefinitionId;
        public readonly string PrincipalId;

        public Properties(string roleID, string SubscriptionId, string PrincipalId)
        {
            this.RoleDefinitionId = $"/subscriptions/{SubscriptionId}/providers/Microsoft.Authorization/roleDefinitions/{roleID}";
            this.PrincipalId = PrincipalId;
        }
    }
}