namespace Pomodorium.TaskManagement.TaskManager;

public class ChangeTaskDescriptionRequest : Request<ChangeTaskDescriptionResponse>
{
    public Guid TaskId { get; set; }

    public string TaskDescription { get; set; }

    public long TaskVersion { get; set; }
}

public class ChangeTaskDescriptionResponse : Response
{
    public ChangeTaskDescriptionResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public ChangeTaskDescriptionResponse() { }
}

public class ChangeTaskDescriptionHandler :
    IRequestHandler<ChangeTaskDescriptionRequest, ChangeTaskDescriptionResponse>
{
    private readonly Repository _repository;

    public ChangeTaskDescriptionHandler(Repository flowtimeRepository)
    {
        _repository = flowtimeRepository;
    }

    public async Task<ChangeTaskDescriptionResponse> Handle(ChangeTaskDescriptionRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetAggregateById<Tasks.Task>(request.TaskId);

        if (task == null)
        {
            throw new EntityNotFoundException();
        }

        task.ChangeDescription(request.TaskDescription);

        await _repository.Save(task, request.TaskVersion);

        var response = new ChangeTaskDescriptionResponse(request.GetCorrelationId()) { };

        return response;
    }
}