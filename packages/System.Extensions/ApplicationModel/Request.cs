using MediatR;

namespace System.ApplicationModel;

public abstract record Request<TResponse> : Request, IRequest<TResponse>
{

}

public abstract record Request : IRequest
{
    protected Guid _correlationId = Guid.NewGuid();
    public Guid GetCorrelationId() => _correlationId;
}
