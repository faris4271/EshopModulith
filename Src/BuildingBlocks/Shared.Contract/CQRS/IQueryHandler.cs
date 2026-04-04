using MediatR;
using Shared.Contract.ResultPattern;

namespace Shared.Contract.CQRS
{
    public interface IQueryHandler<TQuery, TRespons> : IRequestHandler<TQuery, Result<TRespons>> where TQuery : IQuery<TRespons>
    {
    }
}
