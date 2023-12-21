using Pomodorium.Models.TaskManagement.Activities;

namespace Pomodorium.Features.ActivityManager;

public class ActivityDeletionHandler : IRequestHandler<ActivityDeletionRequest, ActivityDeletionResponse>
{
    private readonly Repository _repository;

    public ActivityDeletionHandler(Repository activityRepository)
    {
        _repository = activityRepository;
    }

    public async Task<ActivityDeletionResponse> Handle(ActivityDeletionRequest request, CancellationToken cancellationToken)
    {
        var activity = await _repository.GetAggregateById<Activity>(request.Id) ?? throw new EntityNotFoundException();

        activity.Delete();

        await _repository.Save(activity, request.Version);

        var response = new ActivityDeletionResponse(request.GetCorrelationId());

        return response;
    }
}
