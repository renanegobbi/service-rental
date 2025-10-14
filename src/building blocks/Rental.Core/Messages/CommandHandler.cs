using FluentValidation.Results;
using Rental.Core.Data;

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
            if (!await uow.Commit()) AddError("An error occurred while persisting the data");

            return ValidationResult;
        }
    }
}