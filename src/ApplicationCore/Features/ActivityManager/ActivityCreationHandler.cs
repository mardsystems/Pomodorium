using Pomodorium.Models.TaskManagement.Activities;

namespace Pomodorium.Features.ActivityManager;

public class ActivityCreationHandler : IRequestHandler<ActivityCreationRequest, ActivityCreationResponse>
{
    private readonly Repository _repository;

    public ActivityCreationHandler(Repository activityRepository)
    {
        _repository = activityRepository;
    }

    public async Task<ActivityCreationResponse> Handle(ActivityCreationRequest request, CancellationToken cancellationToken)
    {
        var activity = new Activity(
            request.Name,
            request.StartDateTime,
            request.StopDateTime,
            request.Description);

        await _repository.Save(activity, -1);

        var response = new ActivityCreationResponse(request.GetCorrelationId());

        return response;
    }
}
