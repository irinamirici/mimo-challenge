using Mimo.Api.Dtos;

namespace Mimo.Api.Commands.Handlers
{
    public interface ICommandHandler<TCommand, TResult>
    {
        Result<TResult> Handle(TCommand command);
    }
}
