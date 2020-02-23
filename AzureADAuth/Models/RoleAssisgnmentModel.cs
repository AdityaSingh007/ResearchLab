using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureADAuth.Models
{
    public class RoleAssisgnmentModel
    {
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string StorageAccountName { get; set; }
        public string ContainerName { get; set; }
        public string RoleId { get; set; }
        public string ServicePrincipalObjectId { get; set; }
        public string RoleAssignmentName { get; set; }
    }
}
