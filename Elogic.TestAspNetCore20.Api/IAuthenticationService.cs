using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Elogic.TestAspNetCore20.Api
{
   public interface IAuthenticationService
   {
      bool TryValidateUser(string username, string password, out string token);

      ClaimsPrincipal GetUserFromToken(string token);
   }
}
