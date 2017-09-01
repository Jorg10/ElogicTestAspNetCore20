using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Elogic.TestAspNetCore20.Api
{
   public class AuthenticationService : IAuthenticationService
   {
      private List<User> users = new List<User>
      {
         new User
         {
            Username = "Yuri",
            Password = "test",
            Role = "Admin"
         },
         new User
         {
            Username = "Amina",
            Password = "test",
            Role = "User"
         }
      };

      private Dictionary<string, string> activeTokens = new Dictionary<string, string>();

      public bool TryValidateUser(string username, string password, out string token)
      {
         token = string.Empty;

         var user = users.FirstOrDefault(u => string.Compare(u.Username, username, ignoreCase: true) == 0 && u.Password == password);

         if (user == null)
         {
            return false;
         }

         token = new string(username.Reverse().ToArray());

         activeTokens.TryAdd(token, username);

         return true;
      }

      public ClaimsPrincipal GetUserFromToken(string token)
      {
         var username = activeTokens.FirstOrDefault(x => x.Key == token).Value;

         if (string.IsNullOrWhiteSpace(username))
         {
            return null;
         }

         var user = users.FirstOrDefault(u => string.Compare(u.Username, username, ignoreCase: true) == 0);

         if (user == null)
         {
            return null;
         }

         var identity = new ClaimsIdentity("Stateless");

         identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
         identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Username));
         identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", user.Username));
         identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

         return new ClaimsPrincipal(identity);
      }
   }
}
