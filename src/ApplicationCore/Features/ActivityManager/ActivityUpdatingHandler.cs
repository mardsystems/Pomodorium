using TaskManagement.Models.Activities;

namespace Pomodorium.Features.ActivityManager;

public class ActivityUpdatingHandler : IRequestHandler<ActivityUpdatingRequest, ActivityUpdatingResponse>
{
    private readonly Repository _repository;

    public ActivityUpdatingHandler(Repository activityRepository)
    {
        _repository = activityRepository;
    }

    public async Task<ActivityUpdatingResponse> Handle(ActivityUpdatingRequest request, CancellationToken cancellationToken)
    {
        var activity = await _repository.GetAggregateById<Activity>(request.Id) ?? throw new EntityNotFoundException();

        activity.Update(
            request.Name,
            request.StartDateTime,
            request.StopDateTime,
            request.Description);

        await _repository.Save(activity, request.Version);

        var response = new ActivityUpdatingResponse(request.GetCorrelationId());

        return response;
    }
}
