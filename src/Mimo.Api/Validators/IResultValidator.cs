using FluentValidation;
using Mimo.Api.Dtos;

namespace Mimo.Api.Validators
{
    public interface IResultValidator<T> : IValidator<T>
    {
        new Result Validate(T instanceToValidate);
    }
}