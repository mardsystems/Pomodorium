using Pomodorium.TaskManagement.Activities;

namespace Pomodorium.TaskManagement.ActivityManager;

public class PostActivityRequest : Request<PostActivityResponse>
{
    public string Name { get; set; }

    public DateTime? StartDateTime { get; set; }

    public DateTime? StopDateTime { get; set; }

    public string Description { get; set; }
}

public class PostActivityResponse : Response
{
    public PostActivityResponse(Guid correlationId)
        : base(correlationId)
    {

    }

    public PostActivityResponse()
    {

    }
}

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
