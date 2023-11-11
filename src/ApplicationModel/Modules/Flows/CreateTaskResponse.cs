namespace Pomodorium.Modules.Flows;

public class CreateTaskResponse : Response
{
    public CreateTaskResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public CreateTaskResponse() { }
}
