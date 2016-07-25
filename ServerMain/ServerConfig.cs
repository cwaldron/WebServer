using System;
using System.Linq;
using WebServer.Application;

namespace ServerApp
{
    /// <summary>
    /// Superscribe configuration for Web API
    /// </summary>
    public static class ServerConfig
    {
        //public static HttpConfiguration HttpConfiguration { get; private set; }

        //public static HttpControllerTypeCache ControllerTypeCache { get; private set; }

        //public static IRouteEngine RegisterModules(HttpConfiguration configuration, IRouteEngine engine = null, string qualifier = "")
        public static void RegisterModules(string qualifier = "")
        {
            //engine = RegisterCommon(configuration, qualifier, engine);

            var modules = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                           from type in assembly.GetTypes()
                           where typeof(ApplicationModule).IsAssignableFrom(type) && type != typeof(ApplicationModule)
                           select new { Type = type }).ToList();

            foreach (var module in modules)
            {
                var instance = (ApplicationModule)Activator.CreateInstance(module.Type);
                instance.Initialize();
            }

            //return engine;
        }

        //public static IRouteEngine Register(HttpConfiguration configuration, IRouteEngine engine = null, string qualifier = "")
        //{
        //    return RegisterCommon(configuration, qualifier, engine);
        //}

        //private static IRouteEngine RegisterCommon(HttpConfiguration configuration, string qualifier, IRouteEngine engine = null)
        //{
        //    if (engine == null)
        //    {
        //        engine = RouteEngineFactory.Create();
        //    }

        //    configuration.DependencyResolver = new SuperscribeDependencyAdapter(configuration.DependencyResolver, engine);
        //    configuration.MessageHandlers.Add(new SuperscribeHandler());

        //    var actionSelector = configuration.Services.GetService(typeof(IHttpActionSelector)) as IHttpActionSelector;
        //    var controllerSelector = configuration.Services.GetService(typeof(IHttpControllerSelector)) as IHttpControllerSelector;
        //    var actionInvoker = configuration.Services.GetService(typeof(IHttpActionInvoker)) as IHttpActionInvoker;

        //    configuration.Services.Replace(typeof(IHttpActionSelector), new SuperscribeActionSelectorAdapter(actionSelector));
        //    configuration.Services.Replace(typeof(IHttpControllerSelector), new SuperscribeControllerSelectorAdapter(controllerSelector));
        //    configuration.Services.Replace(typeof(IHttpActionInvoker), new SuperscribeActionInvokerAdapter(actionInvoker));

        //    ControllerTypeCache = new HttpControllerTypeCache(configuration);

        //    var template = "{*wildcard}";
        //    if (!string.IsNullOrEmpty(qualifier))
        //    {
        //        template = qualifier + "/" + template;
        //    }

        //    HttpConfiguration = configuration;

        //    // We need a single default route that will match everything
        //    // configuration.Routes.Clear();
        //    configuration.Routes.MapHttpRoute(
        //        name: "Superscribe",
        //        routeTemplate: template,
        //        defaults: new { });

        //    return engine;
        //}
    }
}
