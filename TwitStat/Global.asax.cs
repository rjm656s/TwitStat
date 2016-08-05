using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TwitStat.Services;

namespace TwitStat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Create the emoji dictionary
            EmojiHandler emojiHandler = new EmojiHandler();
            Dictionary<string, string> emojiDict = emojiHandler.GetEmojiDictionary();

            //Initialize stream on application start
            Task.Factory.StartNew(() => new StreamConnection(emojiDict));
        }
    }
}
