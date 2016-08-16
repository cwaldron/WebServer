using WebServer.Routing;

namespace WebServer.Application
{
    public abstract class ApplicationModule
    {
        protected ApplicationModule()
        {
            Get = new MethodSet(RouteMethod.Get.ToString().ToUpperInvariant(), this);
            Put = new MethodSet(RouteMethod.Put.ToString().ToUpperInvariant(), this);
            Post = new MethodSet(RouteMethod.Post.ToString().ToUpperInvariant(), this);
            Patch = new MethodSet(RouteMethod.Patch.ToString().ToUpperInvariant(), this);
            Delete = new MethodSet(RouteMethod.Delete.ToString().ToUpperInvariant(), this);
        }

        public void Initialize()
        {
            Get.Initialize();
            Put.Initialize();
            Post.Initialize();
            Patch.Initialize();
            Delete.Initialize();
        }

        public MethodSet Get { get; }

        public MethodSet Put { get; }

        public MethodSet Post { get; }

        public MethodSet Patch { get; }

        public MethodSet Delete { get; }
    }
}
