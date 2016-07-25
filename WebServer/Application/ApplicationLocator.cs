using System;

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
            return null;
        }
    }
}