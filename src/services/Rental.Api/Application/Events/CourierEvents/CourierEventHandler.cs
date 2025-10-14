using MediatR;
using Rental.Api.Application.Events.CourierEvent;
using Rental.MessageBus;

namespace Rental.Api.Application.Events
{
    public class CourierEventHandler : INotificationHandler<CourierRegisteredEvent>
    {
        private readonly IMessageBus _bus;

        public CourierEventHandler(IMessageBus bus)
        {
            _bus = bus;
        }

        public Task Handle(CourierRegisteredEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
