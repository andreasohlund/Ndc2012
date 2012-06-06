namespace Backend.Shipping
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;
    using Petshop.Messages;

    public class ShippingSaga:Saga<ShippingSagaData>,
        IAmStartedByMessages<PlaceOrder>,
        IAmStartedByMessages<OrderBilled>,
        IHandleTimeouts<OrderNotBilledInTime>
    {
        public void Handle(PlaceOrder message)
        {
            Data.OrderId = message.OrderId;

            Data.OrderAccepted = true;

            RequestUtcTimeout<OrderNotBilledInTime>(TimeSpan.FromDays(1));

            Ship();
        }
        void Ship()
        {
            if(Data.OrderAccepted && Data.OrderBilled)
            {
                //Bus.Send(new ShipOrder());
                MarkAsComplete();
              
                
            }
        }
        public void Handle(OrderBilled message)
        {
            Data.OrderId = message.OrderId;
            Data.OrderBilled = true;

            Ship();
        }

        public override void ConfigureHowToFindSaga()
        {
            ConfigureMapping<OrderAccepted>(s=>s.OrderId,m=>m.OrderId);
        }

        public void Timeout(OrderNotBilledInTime state)
        {
            if (Data.OrderAccepted && Data.OrderBilled)
                return;

            Console.WriteLine("Order not billed in time:" + Data.OrderId);
            Bus.Send(new NotifyUserThatOrderCantBeBilled());
        }
    }

    public class NotifyUserThatOrderCantBeBilled 
    {
    }

    public class OrderNotBilledInTime
    {
    }

    public class ShippingSagaData : ISagaEntity
    {
        public Guid Id { get; set; }
        public string Originator { get; set; }
        public string OriginalMessageId { get; set; }

        public Guid OrderId { get; set; }

        public bool OrderAccepted { get; set; }

        public bool OrderBilled { get; set; }
    }
}