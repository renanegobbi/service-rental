using FluentValidation.Results;
using MediatR;
using Rental.Core.Interfaces;
using Rental.Core.Messages.Integration;

namespace Rental.Core.Messages
{
    public abstract class Command : Message, IRequest<IResponse>
    {
        public DateTime Timestamp { get; private set; }

        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.UtcNow;
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }

    }
}