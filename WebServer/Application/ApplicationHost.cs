namespace WebServer.Application
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public class ApplicationHost
    {
        internal ApplicationHost()
        {
            ApplicationStartup();
        }

        protected virtual void ApplicationStartup()
        {
            
        }
    }
}