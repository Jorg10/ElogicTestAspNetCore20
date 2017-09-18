using Nancy;
using Nancy.Authentication.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Elogic.TestAspNetCore20.Api
{
   public class UserValidator : IUserValidator
   {
      public ClaimsPrincipal Validate(string username, string password)
      {
         if (username.ToLowerInvariant() == "yuri" && password == "test")
         {
            var identity = new ClaimsIdentity("Basic");

            identity.AddClaim(new Claim(ClaimTypes.Name, "yuri"));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "yuri"));
            identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "yuri"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

            return new ClaimsPrincipal(identity);
         }

         return null;
      }
   }
}
