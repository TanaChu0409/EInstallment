using EInstallment.Domain.Shared;
using MediatR;

namespace EInstallment.Application.Abstractions.Messaging;

public interface IQuery<TResponse>
    : IRequest<Result<TResponse>>
{
}