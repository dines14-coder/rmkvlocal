using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Api.Constants
{
   public class Policy
    {
        public const string TerritoryTokenHolderOnly = "TerritoryAuthorizeAttribute";
        public const string BearerOnly = "BearerTokenAttribute";
        public const string CorsOnly = "CorsPolicy";
        public const string AllowedOrg = "AllowedOrigins";
    }
}
