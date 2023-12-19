using Pomodorium.Models.TaskManagement.Activities;

namespace Pomodorium.Features.ActivityManager;

public class DeleteActivityHandler : IRequestHandler<DeleteActivityRequest, DeleteActivityResponse>
{
    private readonly Repository _repository;

    public DeleteActivityHandler(Repository activityRepository)
    {
        _repository = activityRepository;
    }

    public async Task<DeleteActivityResponse> Handle(DeleteActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = await _repository.GetAggregateById<Activity>(request.Id) ?? throw new EntityNotFoundException();

        activity.Delete();

        await _repository.Save(activity, request.Version);

        var response = new DeleteActivityResponse(request.GetCorrelationId());

        return response;
    }
}
