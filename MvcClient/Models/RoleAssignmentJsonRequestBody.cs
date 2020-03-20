using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcClient.Models
{
    public class RoleAssignmentJsonRequestBody
    {
        public RoleAssignmentJsonRequestBody(Properties properties)
        {
            this.properties = properties;
        }
        public readonly Properties properties;
    }
}