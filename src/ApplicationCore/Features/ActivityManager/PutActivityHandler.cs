using Pomodorium.Models.TaskManagement.Activities;

namespace Pomodorium.Features.ActivityManager;

public class PutActivityHandler : IRequestHandler<PutActivityRequest, PutActivityResponse>
{
    private readonly Repository _repository;

    public PutActivityHandler(Repository activityRepository)
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
