using Pomodorium.Models.TaskManagement.Activities;

namespace Pomodorium.Features.ActivityManager;

public class PostActivityHandler : IRequestHandler<PostActivityRequest, PostActivityResponse>
{
    private readonly Repository _repository;

    public PostActivityHandler(Repository activityRepository)
    {
        _repository = activityRepository;
    }

    public async Task<PostActivityResponse> Handle(PostActivityRequest request, CancellationToken cancellationToken)
    {
        var activity = new Activity(
            request.Name,
            request.StartDateTime,
            request.StopDateTime,
            request.Description);

        await _repository.Save(activity, -1);

        var response = new PostActivityResponse(request.GetCorrelationId()) { };

        return response;
    }
}
