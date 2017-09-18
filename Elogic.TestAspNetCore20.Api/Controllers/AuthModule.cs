using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elogic.TestAspNetCore20.Api.Controllers
{
   public class AuthModule : NancyModule
   {
      public AuthModule(IAuthenticationService authenticationService) : base("/auth/")
      {
         Post("/", async _ =>
         {
            var authenticated = authenticationService.TryValidateUser(
               (string)Request.Form.Username,
               (string)Request.Form.Password,
               out var token);

            if (authenticated)
            {
               return await Response.AsJson(new { Token = token });
            }
            else
            {
               return new Response { StatusCode = HttpStatusCode.Unauthorized };
            }
         });
      }
   }
}
