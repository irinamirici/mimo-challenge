using FluentValidation;
using FluentValidation.Results;
using Mimo.Api.Dtos;
using Mimo.Api.Messages;
using System.Collections.Generic;
using System.Linq;

namespace Mimo.Api.Validators
{
    public abstract class AbstractValidatorBase<T> : AbstractValidator<T>, IResultValidator<T>
    {
        public new Result Validate(T instanceToValidate)
        {
            if (instanceToValidate == null)
            {
                return Result.Fail(WithErrors(string.Empty, Constants.ErrorCodes.BadFormat, ErrorMessages.MissingRequestEntity));
            }
            var validationResult = base.Validate(instanceToValidate);

            return validationResult.Errors.Any()
               ? Result.Fail(validationResult.Errors.Select(MapToResultError))
               : Result.Ok();
        }

        private ResultError MapToResultError(ValidationFailure failure)
        {
            return new ResultError(failure.ErrorCode,
                failure.ErrorMessage,
                failure.PropertyName,
                failure.AttemptedValue);
        }

        protected IEnumerable<ResultError> WithErrors(string propertyName,
          string errorCode,
          string errorMessage)
        {
            return new[] { new ResultError(errorCode, errorMessage, propertyName) };
        }
    }
}
