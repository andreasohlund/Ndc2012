namespace Backend
{
    using System;
    using NServiceBus;
    using Petshop.Messages;
    using Raven.Client.Document;

    public class PlaceOrderHandler:IHandleMessages<PlaceOrder>
    {
        public IBus Bus { get; set; }

        public void Handle(PlaceOrder message)
        {
            Bus.Defer(TimeSpan.FromSeconds(30), new SendOutOrderRevireForm());
        }
    }

    public class SendOutOrderRevireFormHandler : IHandleMessages<SendOutOrderRevireForm>
     {
        public void Handle(SendOutOrderRevireForm message)
        {

            Console.WriteLine("Send the review form");
        }
     }

    public class PlaceOrderHandler2 : IHandleMessages<PlaceOrder>
    {
        public void Handle(PlaceOrder message)
        {
            var store = new DocumentStore
            {
                Url = "http://localhost:8080"
            };

            store.Initialize();

            using (var session = store.OpenSession())
            {
                session.Store(message);
                session.SaveChanges();
            }
            Console.WriteLine("Store in db: " + message.ProductId);

           // throw new Exception("Failed to send email");
        }
    }
}