using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace PRA
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            StripeConfiguration.ApiKey = "sk_test_51LFsR2ICrnxhV7iWA4a9FpeljppjdsEG66yHb3B1DcIU6B862iltXqdKAZG0i6EfgSOqurqKwi24BgR5N76kfSqV00S8cRoXdz";
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }


        protected void Application_Error()
        {
            string error = Server?.GetLastError()?.GetBaseException()?.Message.Trim().Replace("\n", "").Replace("\r", "");
            Response.Redirect("~/Error/Index?message=" + error);
        }
    }
}
