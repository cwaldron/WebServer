using System;
using System.Collections.Generic;
using System.Linq;

namespace WebServer.Application
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public class ApplicationLocator
    {
        public IApplication FindApplication()
        {
            return FindApplication(FindApplicationInternal);
        }

        public IEnumerable<ApplicationModule> FindModules()
        {
            return FindModules(FindModulesInternal);
        }

        public IApplication FindApplication(Func<IApplication> locater)
        {
            return locater();
        }

        public IEnumerable<ApplicationModule> FindModules(Func<IEnumerable<ApplicationModule>> locater)
        {
            return locater();
        }

        private static IApplication FindApplicationInternal()
        {
            // Get assemblies.
            var assems = AppDomain.CurrentDomain.GetAssemblies();

            // Load application.
            // ReSharper disable once PossibleMultipleEnumeration
            Type appType = assems.Select(assem => assem.GetTypes().Where(x => x.IsSubclassOf(typeof(Application)))).SelectMany(y => y).Single();
            return (IApplication)Activator.CreateInstance(appType);
        }


        private static ModuleCollection FindModulesInternal()
        {
            // Get assemblies.
            var assems = AppDomain.CurrentDomain.GetAssemblies();

            // Load modules.
            var modules = new ModuleCollection();
            // ReSharper disable once PossibleMultipleEnumeration
            foreach (Type moduleType in assems.Select(assem => assem.GetTypes().Where(x => x.IsSubclassOf(typeof(ApplicationModule)))).SelectMany(y => y))
            {
                // Module types found.
                modules.Add((ApplicationModule)Activator.CreateInstance(moduleType));
            }

            return modules;
        }
    }
}