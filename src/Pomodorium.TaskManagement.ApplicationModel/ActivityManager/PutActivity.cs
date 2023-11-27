using Pomodorium.TaskManagement.Activities;

namespace Pomodorium.TaskManagement.ActivityManager;

public class PutActivityRequest : Request<PutActivityResponse>
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public string Description { get; set; }

    public long Version { get; set; }
}

public class PutActivityResponse : Response
{
    public PutActivityResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public PutActivityResponse()
    {

    }
}

public class ActivityApplication : IRequestHandler<PutActivityRequest, PutActivityResponse>
{
    private readonly Repository _repository;

    public ActivityApplication(Repository activityRepository)
    {
        _repository = activityRepository;
    }

    public async Task<PutActivityResponse> Handle(PutActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await _repository.GetAggregateById<Activity>(request.Id);

        if (activity == null)
        {
            throw new EntityNotFoundException();
        }

        activity.Update(
            request.Name,
            request.StartDateTime,
            request.StopDateTime,
            request.Description);

        await _repository.Save(activity, request.Version);

        var response = new PutActivityResponse(request.GetCorrelationId()) { };

        return response;
    }
}
