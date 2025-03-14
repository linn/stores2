namespace Linn.Stores2.IoC
{
    using Linn.Common.Messaging.RabbitMQ.Configuration;
    using Linn.Stores2.Messaging.Messages;

    using Microsoft.Extensions.DependencyInjection;
    using RabbitMQ.Client.Events;

    public static class MessagingExtensions
    {
        public static IServiceCollection AddRabbitConfiguration(this IServiceCollection services)
        {
            // all the routing keys the Listener cares about need to be registered here:
            var routingKeys = new[] { ThingMessage.RoutingKey };

            return services
                .AddSingleton<ChannelConfiguration>(d => new ChannelConfiguration("stores2", routingKeys))
                .AddSingleton(d => new AsyncEventingBasicConsumer(d.GetService<ChannelConfiguration>()?.ConsumerChannel));
        }

        public static IServiceCollection AddMessageHandlers(this IServiceCollection services)
        {
            // register handlers for different message types
            return services;
        }

        public static IServiceCollection AddMessageDispatchers(this IServiceCollection services)
        {
            // register dispatchers for different message types:
            return services;
        }
    }
}
