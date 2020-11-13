using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjeTakip
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
                name: "Admin",
                url: "Admin",
                defaults: new { controller = "Login", action = "Login"}
            );

            routes.MapRoute(
                name: "PassReset",
                url: "ResetPassword/{id}",
                defaults: new { controller = "Profil", action = "ResetPassword"}
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Anasayfa", action = "Index" , id = UrlParameter.Optional}
            );
        }
    }
}


/*
 * 
 * 
 * 
 *   <system.web>
    <authentication mode="Forms">
      <forms cookieless="UseCookies" loginUrl="~/Login/Login" defaultUrl="~/" slidingExpiration="true"></forms>    
    </authentication>
    <roleManager defaultProvider="MyProvider" enabled="true">
      <providers>
        <clear />
        <add name="MyProvider" type="ProjeTakip.MyRoleProvider.SiteRole" />
      </providers>
    </roleManager>

    <compilation debug="true" targetFramework="4.5" />
    <httpRuntime targetFramework="4.5" />
    <sessionState timeout="40"></sessionState>
  </system.web>
*/