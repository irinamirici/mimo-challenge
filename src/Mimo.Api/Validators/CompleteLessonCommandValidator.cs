using FluentValidation;
using Mimo.Api.Commands;
using Mimo.Api.Messages;
using System;

namespace Mimo.Api.Validators
{
    public class CompleteLessonCommandValidator : AbstractValidatorBase<CompleteLessonCommand>
    {
        public CompleteLessonCommandValidator()
        {
            RuleFor(x => x.StartTime)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithErrorCode(Constants.ErrorCodes.Empty)
                .WithMessage(ErrorMessages.FieldCannotBeEmpty)

                .Must(BeInThePast)
                .WithErrorCode(Constants.ErrorCodes.DateMustBeInThePast)
                .WithMessage(ErrorMessages.BeInThePast);


            RuleFor(x => x.EndTime)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithErrorCode(Constants.ErrorCodes.Empty)
                .WithMessage(ErrorMessages.FieldCannotBeEmpty)

                .Must(BeInThePast)
                .WithErrorCode(Constants.ErrorCodes.DateMustBeInThePast)
                .WithMessage(ErrorMessages.BeInThePast)

                .Must((command, prop) => BeLowerThanEndDate(command))
                .WithErrorCode(Constants.ErrorCodes.EndTimeMustBeGreaterThenStartTime)
                .WithMessage(ErrorMessages.EndTimeMustBeGreaterThenStartTime);
        }

        private bool BeInThePast(DateTime? arg)
        {
            return arg < DateTime.UtcNow;
        }

        private bool BeLowerThanEndDate(CompleteLessonCommand arg)
        {
            return arg.StartTime < arg.EndTime;
        }
    }
}
