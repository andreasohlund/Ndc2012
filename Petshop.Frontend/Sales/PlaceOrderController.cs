namespace Petshop.Frontend.Sales
{
    using System;
    using System.Web.Http;
    using Messages;
    using NServiceBus;

    public class PlaceOrderController:ApiController
    {
        public IBus Bus { get; set; }

        public string Get()
        {
            var productId = Guid.NewGuid();

            Bus.Send(new PlaceOrder
                         {
                             ProductId = productId
                         });
            return "Hello ndc";
        }
    }
}