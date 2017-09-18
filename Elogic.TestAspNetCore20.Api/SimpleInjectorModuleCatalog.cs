using Nancy;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elogic.TestAspNetCore20.Api
{
   public sealed class SimpleInjectorModuleCatalog : INancyModuleCatalog
   {
      private readonly Container container;

      public SimpleInjectorModuleCatalog(Container container) { this.container = container; }

      public INancyModule GetModule(Type moduleType, NancyContext context) =>
          (INancyModule)this.container.GetInstance(moduleType);

      public IEnumerable<INancyModule> GetAllModules(NancyContext context) =>
          from r in this.container.GetCurrentRegistrations()
          where typeof(INancyModule).IsAssignableFrom(r.ServiceType)
          select (INancyModule)r.GetInstance();
   }
}
