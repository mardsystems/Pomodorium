using MediatR;
using System.DomainModel;

namespace Pomodorium.Modules.Activities;

public class ActivityApplication :
    IRequestHandler<PostActivityRequest, PostActivityResponse>,
    IRequestHandler<PutActivityRequest, PutActivityResponse>,
    IRequestHandler<DeleteActivityRequest, DeleteActivityResponse>
{
    private readonly Repository _repository;

    public ActivityApplication(Repository activityRepository)
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
