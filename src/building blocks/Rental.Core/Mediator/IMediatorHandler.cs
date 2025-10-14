using FluentValidation.Results;
using Rental.Core.Messages;

namespace Rental.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T eventItem) where T : Event;
        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
    }
}