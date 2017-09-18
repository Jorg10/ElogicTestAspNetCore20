using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Elogic.TestAspNetCore20.Api
{
   public class AuthenticationService : IAuthenticationService
   {
      private const string issuer = "self";
      private const string audience = "https://www.testaspnetcore20.com";

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

      public bool TryValidateUser(string username, string password, out string token)
      {
         token = string.Empty;

         var user = users.FirstOrDefault(u => string.Compare(u.Username, username, ignoreCase: true) == 0 && u.Password == password);

         if (user == null)
         {
            return false;
         }

         var claims = new List<Claim>
         {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", user.Username),
            new Claim(ClaimTypes.Role, user.Role)
         };

         var securityKey = GetSecurityKey();

         var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

         var jwtToken = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            signingCredentials: credentials,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(30));

         var handler = new JwtSecurityTokenHandler();

         token = handler.WriteToken(jwtToken);

         return true;
      }

      public ClaimsPrincipal GetUserFromToken(string token)
      {
         if (string.IsNullOrWhiteSpace(token))
         {
            return null;
         }

         var handler = new JwtSecurityTokenHandler();

         var validationParameters = new TokenValidationParameters
         {
            ValidIssuer = issuer,
            ValidAudience = audience
         };

         validationParameters.IssuerSigningKey = GetSecurityKey();

         var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);

         return principal;
      }
      private static SecurityKey GetSecurityKey()
      {
         var input = "mypassword";
         var securityKey = new byte[input.Length * sizeof(char)];
         Buffer.BlockCopy(input.ToCharArray(), 0, securityKey, 0, securityKey.Length);
         return new SymmetricSecurityKey(securityKey);
      }
   }
}
