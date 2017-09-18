using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elogic.TestAspNetCore20.Api
{
   public class HomeService : IHomeService
   {
      public string GetMessage()
      {
         return "Hello World, it's Nancy on .NET Core";
      }
   }
}
