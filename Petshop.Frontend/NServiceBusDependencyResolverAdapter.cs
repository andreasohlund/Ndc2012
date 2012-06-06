namespace Petshop.Frontend
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Dependencies;
    using NServiceBus;
    using NServiceBus.ObjectBuilder;

    public class NServiceBusDependencyResolverAdapter : IDependencyResolver
    {
        readonly IBuilder builder;

        public NServiceBusDependencyResolverAdapter(IBuilder builder)
        {
            this.builder = builder;
        }

        public object GetService(Type serviceType)
        {
            if (Configure.Instance.Configurer.HasComponent(serviceType))
                return builder.Build(serviceType);
            else
                return null;
        }

        
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return builder.BuildAll(serviceType);
        }

        public IDependencyScope BeginScope()
        {
            return new NServiceBusDependencyResolverAdapter(builder.CreateChildBuilder());
        }

        public void Dispose()
        {
            builder.Dispose();
        }
    }

    public static class ConfigureWeAPiDependencyInjection
    {
        public static Configure ForWebApi(this Configure configure)
        {

            // Find every controller class so that we can register it
            var controllers = Configure.TypesToScan
                .Where(t => typeof(ApiController).IsAssignableFrom(t));

            // Register each controller class with the NServiceBus container
            foreach (Type type in controllers)
                configure.Configurer.ConfigureComponent(type, DependencyLifecycle.InstancePerCall);

            // Set the MVC dependency resolver to use our resolver
            GlobalConfiguration.Configuration.DependencyResolver =
                new NServiceBusDependencyResolverAdapter(configure.Builder);
            return configure;
        }
    }

}