using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Rental.MessageBus.Serializers;

namespace Rental.MessageBus
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IMessageBus>(sp =>
            {
                var bus = new MessageBus(connectionString);

                var advancedBus = bus.AdvancedBus;
                if (advancedBus != null)
                {
                    var conventions = new Conventions(new ShortTypeNameSerializer());

                    conventions.QueueNamingConvention = (type, subscriptionId) =>
                        subscriptionId;

                    conventions.ExchangeNamingConvention = type =>
                        $"rental.{type.Name.Replace("IntegrationEvent", "").ToLower()}";

                    conventions.TopicNamingConvention = type =>
                        $"rental.{type.Name.Replace("IntegrationEvent", "").ToLower()}";
                }

                return bus;
            });

            return services;
        }
    }
}


