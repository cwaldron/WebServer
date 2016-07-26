using WebServer.Routing;

namespace WebServer
{
    public abstract class Module<T> where T : RouteData
    {
        protected Module()
        {
            Get = new MethodSet<T>(RouteMethod.Get.ToString().ToUpperInvariant());
            Put = new MethodSet<T>(RouteMethod.Put.ToString().ToUpperInvariant());
            Post = new MethodSet<T>(RouteMethod.Post.ToString().ToUpperInvariant());
            Patch = new MethodSet<T>(RouteMethod.Patch.ToString().ToUpperInvariant());
            Delete = new MethodSet<T>(RouteMethod.Delete.ToString().ToUpperInvariant());
        }

        public void Initialize()
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
    }
}
