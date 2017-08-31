using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Elogic.TestAspNetCore20.Api.Controllers
{
   public class SecureModule : NancyModule
   {
      public SecureModule()
      {
         Get("/secure", _ =>
         {
            this.RequiresClaims(c => c.Type == ClaimTypes.Role && c.Value == "Admin");

            return "Secure api";
         });
      }
   }
}
