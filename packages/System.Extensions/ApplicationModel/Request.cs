using MediatR;

namespace System.ApplicationModel;

public abstract class Request<TResponse> : Request, IRequest<TResponse>
{

}

public abstract class Request : IRequest
{
    protected Guid _correlationId = Guid.NewGuid();
    public Guid GetCorrelationId() => _correlationId;
}
