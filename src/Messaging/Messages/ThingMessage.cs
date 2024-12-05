namespace Linn.Stores2.Messaging.Messages
{
    using Linn.Common.Messaging.RabbitMQ.Messages;

    using RabbitMQ.Client.Events;

    public class ThingMessage : RabbitMessage
    {
        public const string RoutingKey = "thing";

        public ThingMessage(BasicDeliverEventArgs e)
            : base(e)
        {
        }
    }
}
