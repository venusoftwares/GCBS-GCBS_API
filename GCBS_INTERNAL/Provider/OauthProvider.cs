using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace GCBS_INTERNAL.Provider
{
    public class OauthProvider : OAuthAuthorizationServerProvider
    {
        
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(() => context.Validated());
        }    
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            if (!string.IsNullOrEmpty(context.UserName) && !string.IsNullOrEmpty(context.Password))
            {     
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim("DeviceUniqueId", context.Password));
                identity.AddClaim(new Claim("LoggedOn", DateTime.Now.ToString()));
                await Task.Run(() => context.Validated(identity));
            }
            else
            {
                context.SetError("Wrong Crendtials", "Provided EmpId and DeviceUniqueId is incorrect");
            }
            return;
        }

    }
}