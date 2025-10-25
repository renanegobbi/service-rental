using MediatR;
using Rental.Core.Interfaces;
using Rental.Core.Messages;

namespace Rental.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IResponse> SendCommand<T>(T comand) where T : Command
        {
            return await _mediator.Send(comand);
        }

        public async Task PublishEvent<T>(T eventItem) where T : Event
        {
            Console.WriteLine($"[MediatorHandler] Publishing event: {eventItem.GetType().Name}");
            await _mediator.Publish(eventItem);
        }

        public async Task<TResult> SendQuery<TResult>(IRequest<TResult> query)
        {
            return await _mediator.Send(query);
        }
    }
}