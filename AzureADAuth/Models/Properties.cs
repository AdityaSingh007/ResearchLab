using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureADAuth.Models
{

    public class RoleAssignmentJsonRequestBody
    {
        public RoleAssignmentJsonRequestBody(Properties properties)
        {
            this.properties = properties;
        }
        public readonly Properties properties;
    }
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
