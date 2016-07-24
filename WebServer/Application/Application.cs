namespace WebServer.Application
{
    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public class Application
    {
        internal Application()
        {
            ApplicationStartup();
        }

        protected virtual void ApplicationStartup()
        {
            
        }
    }
}