using System.Collections.Generic;
using WebServer.Routing;

namespace WebServer.Application
{
    public abstract class ApplicationModule
    {
        protected ApplicationModule()
        {
            var setters = new[]
            {
                new RouteMethodSetter(RouteMethod.Get.ToString().ToUpperInvariant()),
                new RouteMethodSetter(RouteMethod.Post.ToString().ToUpperInvariant()),
                new RouteMethodSetter(RouteMethod.Put.ToString().ToUpperInvariant()),
                new RouteMethodSetter(RouteMethod.Patch.ToString().ToUpperInvariant()),
                new RouteMethodSetter(RouteMethod.Delete.ToString().ToUpperInvariant())
            };

            Get = setters[(int)RouteMethod.Get];
            Post = setters[(int)RouteMethod.Post];
            Put = setters[(int)RouteMethod.Put];
            Patch = setters[(int)RouteMethod.Patch];
            Delete = setters[(int)RouteMethod.Delete];
            Setters = setters;
        }

        public void Initialize()
        {
            Get.Initialize();
            Put.Initialize();
            Post.Initialize();
            Patch.Initialize();
            Delete.Initialize();
        }

        public RouteMethodSetter Get { get; }

        public RouteMethodSetter Put { get; }

        public RouteMethodSetter Post { get; }

        public RouteMethodSetter Patch { get; }

        public RouteMethodSetter Delete { get; }

        internal IEnumerable<RouteMethodSetter> Setters { get; }
    }
}
