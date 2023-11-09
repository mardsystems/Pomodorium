using MediatR;
using Pomodorium.Modules.Pomos;
using System.DomainModel;

namespace Pomodorium.Modules.Flows;

public class FlowtimeApplication :
    IRequestHandler<CreateFlowtimeRequest, CreateFlowtimeResponse>,
    IRequestHandler<StartFlowtimeRequest, StartFlowtimeResponse>,
    IRequestHandler<StopFlowtimeRequest, StopFlowtimeResponse>,
    IRequestHandler<ChangeTaskDescriptionRequest, ChangeTaskDescriptionResponse>,
    IRequestHandler<ArchiveFlowtimeRequest, ArchiveFlowtimeResponse>
{
    private readonly Repository _repository;

    public FlowtimeApplication(Repository flowtimeRepository)
    {
        _repository = flowtimeRepository;
    }

    public async Task<CreateFlowtimeResponse> Handle(CreateFlowtimeRequest request, CancellationToken cancellationToken)
    {
        Task task;

        if (request.TaskId.HasValue)
        {
            task = await _repository.GetAggregateById<Task>(request.TaskId.Value);

            if (task.Description != request.TaskDescription)
            {
                task.ChangeDescription(request.TaskDescription);

                await _repository.Save(task, request.TaskVersion.Value);

                task = await _repository.GetAggregateById<Task>(request.TaskId.Value);
            }
        }
        else
        {
            task = new Task(request.TaskDescription);

            await _repository.Save(task, -1);
        }

        var flowtime = new Flowtime(task);

        await _repository.Save(flowtime, -1);

        var response = new CreateFlowtimeResponse(request.GetCorrelationId()) { };

        return response;
    }

    public async Task<StartFlowtimeResponse> Handle(StartFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.Id);

        if (flowtime == null)
        {
            throw new EntityNotFoundException();
        }

        flowtime.Start(request.StartDateTime);

        await _repository.Save(flowtime, request.Version);

        var response = new StartFlowtimeResponse(request.GetCorrelationId()) { };

        return response;
    }

    public async Task<StopFlowtimeResponse> Handle(StopFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.Id);

        if (flowtime == null)
        {
            throw new EntityNotFoundException();
        }

        flowtime.Stop(request.StopDateTime);

        await _repository.Save(flowtime, request.Version);

        var response = new StopFlowtimeResponse(request.GetCorrelationId()) { };

        return response;
    }

    public async Task<ChangeTaskDescriptionResponse> Handle(ChangeTaskDescriptionRequest request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetAggregateById<Task>(request.TaskId);

        if (task == null)
        {
            throw new EntityNotFoundException();
        }

        task.ChangeDescription(request.TaskDescription);

        await _repository.Save(task, request.TaskVersion);

        var response = new ChangeTaskDescriptionResponse(request.GetCorrelationId()) { };

        return response;
    }

    public async Task<ArchiveFlowtimeResponse> Handle(ArchiveFlowtimeRequest request, CancellationToken cancellationToken)
    {
        var flowtime = await _repository.GetAggregateById<Flowtime>(request.Id);

        if (flowtime == null)
        {
            throw new EntityNotFoundException();
        }

        flowtime.Archive();

        await _repository.Save(flowtime, request.Version);

        var response = new ArchiveFlowtimeResponse(request.GetCorrelationId()) { };

        return response;
    }
}