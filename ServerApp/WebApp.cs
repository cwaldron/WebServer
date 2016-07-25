using System;
using WebServer.Application;
using WebServer.Routing;

namespace ServerApp
{
    public class WebApp : Application
    {
        public override void Startup()
        {
            Routes.MapRoute(
                name: "BlogPostMediaRoute",
                routeTemplate: "sites/{siteId}/blog/posts/{postId}/media/{id}",
                defaults: new {controller = "blogpostmedia", id = RouteParameter.Optional})

            .MapRoute(
                name: "BlogTagsRoute",
                routeTemplate: "sites/{siteId}/blog/tags",
                defaults: new { controller = "blogtags" })

            .MapRoute(
                name: "TagsRoute",
                routeTemplate: "sites/{siteId}/tags",
                defaults: new { controller = "tags" });
        }

        public override void GetEnvironment()
        {
            throw new NotImplementedException();
        }
    }
}
