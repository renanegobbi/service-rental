using FluentValidation.Results;
using Rental.Core.Messages;

namespace Rental.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;
        Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
    }
}