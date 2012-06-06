namespace Petshop.Messages
{
    using System;

    public class PlaceOrder
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
    }
}