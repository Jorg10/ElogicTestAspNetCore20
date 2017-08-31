using Nancy;
using Nancy.Authentication.Basic;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elogic.TestAspNetCore20.Api
{
   public class Bootstrapper : DefaultNancyBootstrapper
   {
      protected override void ApplicationStartup(TinyIoCContainer nancy, IPipelines pipelines)
      {
         // Create Simple Injector container
         var container = new Container();
         container.Options.DefaultScopedLifestyle = new SimpleInjector.Lifestyles.AsyncScopedLifestyle();

         // Register application components here, e.g.:
         container.Register<IHomeService, HomeService>();
         container.Register<IUserValidator, UserValidator>();

         // Register Nancy modules.
         foreach (var nancyModule in this.Modules) container.Register(nancyModule.ModuleType);

         // Cross-wire Nancy abstractions that application components require (if any). e.g.:
         //container.Register(nancy.Resolve<IModelValidator>);

         // Check the container.
         container.Verify();

         // Hook up Simple Injector in the Nancy pipeline.
         nancy.Register(typeof(INancyModuleCatalog), new SimpleInjectorModuleCatalog(container));
         nancy.Register(typeof(INancyContextFactory), new SimpleInjectorScopedContextFactory(
             container, nancy.Resolve<INancyContextFactory>()));

         pipelines.EnableBasicAuthentication(
            new BasicAuthenticationConfiguration(
               container.GetInstance<IUserValidator>(), "MyRealm"));
      }
   }
}
