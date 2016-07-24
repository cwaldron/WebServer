namespace WebServer.Application
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public class ApplicationLocator
    {
        internal ApplicationLocator()
        {
            ApplicationStartup();
        }

        protected virtual void ApplicationStartup()
        {
            
        }
    }
}