namespace Pomodorium;

public abstract class Request
{
    protected Guid _correlationId = Guid.NewGuid();
    public Guid GetCorrelationId() => _correlationId;
}
