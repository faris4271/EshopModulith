using MediatR;
using Shared.Contract.ResultPattern;

namespace Shared.Contract.CQRS
{
    public interface IQuery<TRespons> : IRequest<Result<TRespons>>
    {
    }

}
