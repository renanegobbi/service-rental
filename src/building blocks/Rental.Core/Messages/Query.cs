using FluentValidation.Results;
using MediatR;

namespace Rental.Core.Messages
{
    public abstract class Query<TResponse> : IRequest<TResponse>
    {
        public ValidationResult ValidationResult { get; protected set; }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
