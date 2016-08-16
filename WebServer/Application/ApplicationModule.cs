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
                new MethodSet(RouteMethod.Get.ToString().ToUpperInvariant()),
                new MethodSet(RouteMethod.Post.ToString().ToUpperInvariant()),
                new MethodSet(RouteMethod.Put.ToString().ToUpperInvariant()),
                new MethodSet(RouteMethod.Patch.ToString().ToUpperInvariant()),
                new MethodSet(RouteMethod.Delete.ToString().ToUpperInvariant())
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

        public MethodSet Get { get; }

        public MethodSet Put { get; }

        public MethodSet Post { get; }

        public MethodSet Patch { get; }

        public MethodSet Delete { get; }

        internal IEnumerable<MethodSet> Setters { get; }
    }
}
