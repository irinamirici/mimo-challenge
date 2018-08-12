using FluentValidation;
using Mimo.Api.Commands;
using Mimo.Api.Messages;

namespace Mimo.Api.Validators
{
    public class CreateCourseCommandValidator : AbstractValidatorBase<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithErrorCode(Constants.ErrorCodes.Empty)
                .WithMessage(ErrorMessages.FieldCannotBeEmpty)
                
                .MaximumLength(50)
                .WithErrorCode(Constants.ErrorCodes.InvalidLength)
                .WithMessage(ErrorMessages.LengthLowerThen);

            RuleFor(x => x.Description)
               .NotEmpty()
               .WithErrorCode(Constants.ErrorCodes.Empty)
               .WithMessage(ErrorMessages.FieldCannotBeEmpty)

               .MaximumLength(200)
               .WithErrorCode(Constants.ErrorCodes.InvalidLength)
               .WithMessage(ErrorMessages.LengthLowerThen);
        }
    }
}
