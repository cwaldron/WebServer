using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebServer.Routing;

namespace WebServer.Application
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public class ApplicationLocator : SingletonBase<ApplicationLocator>
    {
        private IApplication _application;

        public static IApplication Application => Instance._application ?? FindApplication();

        private ApplicationLocator()
        {
        }

        public static IApplication FindApplication()
        {
            return FindApplication(DefaultLocater);
        }

        private static IApplication FindApplication(Func<IApplication> locater)
        {
            Instance._application = locater();
            return Instance._application;
        }

        private static IApplication DefaultLocater()
        {
            var assems = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var modules in assems.Select(assem => assem.GetTypes().Where(x => x.IsSubclassOf(typeof(ApplicationModule)))))
            {
                // Module types found.
                foreach (var type in modules)
                {
                    Router.RegisterModule((ApplicationModule)Activator.CreateInstance(type));
                }
            }

            return null;
        }
    }
}