using MediatR;
using Shared.Contract.ResultPattern;

namespace Shared.Contract.CQRS
{
    public interface ICommand : IRequest<Result>, IBaseCommand;


    public interface ICommand<TRequest> : IRequest<Result<TRequest>>, IBaseCommand;

    public interface IBaseCommand;

}
