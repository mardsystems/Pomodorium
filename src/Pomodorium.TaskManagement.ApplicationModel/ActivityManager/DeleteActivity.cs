using Pomodorium.TaskManagement.Activities;

namespace Pomodorium.TaskManagement.ActivityManager;

public class DeleteActivityRequest : Request<DeleteActivityResponse>
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}

public class DeleteActivityResponse : Response
{
    public DeleteActivityResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public DeleteActivityResponse()
    {

    }
}

public class DeleteActivityHandler : IRequestHandler<DeleteActivityRequest, DeleteActivityResponse>
{
    private readonly Repository _repository;

    public DeleteActivityHandler(Repository activityRepository)
    {
        _repository = activityRepository;
    }

    public async Task<DeleteActivityResponse> Handle(DeleteActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await _repository.GetAggregateById<Activity>(request.Id);

        if (activity == null)
        {
            throw new EntityNotFoundException();
        }

        activity.Delete();

        await _repository.Save(activity, request.Version);

        var response = new DeleteActivityResponse(request.GetCorrelationId()) { };

        return response;
    }
}
