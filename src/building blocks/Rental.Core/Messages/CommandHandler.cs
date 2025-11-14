using FluentValidation.Results;
using Rental.Core.Data;
using Rental.Core.Interfaces;
using Rental.Core.Responses;

namespace Rental.Core.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string mensagem)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, mensagem));
        }

        protected async Task<ValidationResult> PersistData(IUnitOfWork uow)
        {
            if (!await uow.CommitTransaction()) AddError("An error occurred while persisting the data");

            return ValidationResult;
        }

        protected IResponse FailResponse(string? message = null)
        {
            if (ValidationResult.Errors.Any())
                return Response.Fail(ValidationResult.Errors.Select(e => e.ErrorMessage).ToArray());

            if (!string.IsNullOrEmpty(message))
                return Response.Fail(message);

            return Response.Fail("An unexpected error occurred.");
        }
    }
}