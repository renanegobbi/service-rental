using MediatR;
using Rental.Core.Interfaces;
using Rental.Core.Messages;

namespace Rental.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T eventItem) where T : Event;
        Task<IResponse> SendCommand<T>(T command) where T : Command;
        Task<TResult> SendQuery<TResult>(IRequest<TResult> query);
    }
}