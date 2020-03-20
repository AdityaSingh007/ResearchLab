using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcClient.Models
{
    public class RoleAssignmentModel
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