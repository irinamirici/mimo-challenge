using Mimo.Api.Dtos;

namespace Mimo.Api.Queries.Handlers
{
    public interface IQueryHandler<TQuery, TResult>
    {
        Result<TResult> Handle(TQuery query);
    }
}
