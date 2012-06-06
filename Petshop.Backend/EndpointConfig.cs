using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Backend
{
    using NServiceBus;
    using NServiceBus.Unicast.Queuing;
    using NServiceBus.Unicast.Transport;

    public class EndpointConfig:IConfigureThisEndpoint,AsA_Server,IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                .DefaultBuilder()
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.StartsWith("Petshop.Messages"));

        }
    }

}
