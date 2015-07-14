using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PresenceKit
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            //Registering Routes
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.MapRoute("Presence", "presence", new { controller = "Home", action = "Presence"});
            routes.MapRoute("Users", "users", new { controller = "Home", action = "Users" });
            routes.MapRoute("Chats", "chats", new { controller = "Home", action = "Chats" });
            routes.MapRoute("Default", "{*path}", new { controller = "Home", action = "Index" });
        }
    }
}
