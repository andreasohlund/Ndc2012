using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;

namespace Petshop.Frontend
{
    using System.Web.Mvc;
    using NServiceBus;

    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Configure.With()
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Petshop.Messages"))
                .DefaultBuilder()
                .ForWebApi()
                .XmlSerializer()
                .MsmqTransport()
                .UnicastBus()
                .CreateBus()
                .Start(() => Configure.Instance
                    .ForInstallationOn<NServiceBus.Installation.Environments.Windows>()
                    .Install());

        }
    }
}