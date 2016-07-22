using WebServer.Routing;

namespace WebServer
{
    public class Module : Module<RouteData>
    {
    }

    public class Module<T> where T : RouteData
    {
        public Module()
        {
            Get = new MethodSet<T>("GET");
            Put = new MethodSet<T>("PUT");
            Post = new MethodSet<T>("POST");
            Patch = new MethodSet<T>("PATCH");
            Delete = new MethodSet<T>("DELETE");
            Expiry = new MethodSet<T>("EXPIRY");
            Authorize = new MethodSet<T>("AUTH");
        }

        public void Initialise()
        {
            Get.Initialize();
            Put.Initialize();
            Post.Initialize();
            Patch.Initialize();
            Delete.Initialize();
        }

        public MethodSet<T> Get { get; }

        public MethodSet<T> Put { get; }

        public MethodSet<T> Post { get; }

        public MethodSet<T> Patch { get; }

        public MethodSet<T> Delete { get; }

        public MethodSet<T> Expiry { get; }

        public MethodSet<T> Authorize { get; }

    }
}
