using EInstallment.Domain.Shared;
using MediatR;

namespace EInstallment.Application.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}