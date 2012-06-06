namespace Petshop.Frontend.Sales
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Messages;
    using NServiceBus;

    public class PlaceOrderController:ApiController,IHandleMessages<OrderAccepted>
    {
        public IBus Bus { get; set; }

        public string Get()
        {
            var productId = Guid.NewGuid();

            Bus.Send(new PlaceOrder
                         {
                             OrderId = Guid.NewGuid(),
                             ProductId = productId
                         });
            return string.Join(";",orders);
        }

        public void Handle(OrderAccepted message)
        {
            orders.Add(message.OrderId);
        }

        static List<Guid> orders = new List<Guid>();
    }
}