using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elogic.TestAspNetCore20.Api.Controllers
{
   public class HomeModule : NancyModule
   {
      private readonly IHomeService homeService;

      public HomeModule(IHomeService homeService)
      {
         this.homeService = homeService;

         var message = homeService.GetMessage();

         Get("/", args => message);
      }
   }
}