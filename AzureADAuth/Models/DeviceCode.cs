using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureADAuth.Models
{
    public class DeviceCodeAuthorizationResponse
    {
        public string UserCode { get; set; }
        public string DeviceCode { get; set; }
        public string VerificationUri { get; set; }
        public string VerificationUriComplete { get; set; }

        public string error { get; set; }
    }
}
