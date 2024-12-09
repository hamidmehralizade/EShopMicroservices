using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface ICommnad : ICommand<Unit>
    {
    }

    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}
