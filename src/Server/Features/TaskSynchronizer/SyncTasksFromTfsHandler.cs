﻿using MediatR;
using Pomodorium.Features.Settings;
using Pomodorium.Features.TaskManager;
using Pomodorium.TaskManagement.Model.Integrations;
using Pomodorium.TeamFoundationServer;
using System.DomainModel;

namespace Pomodorium.Features.TaskSynchronizer;

public class SyncTasksFromTfsHandler : IRequestHandler<SyncTasksFromTfsRequest, SyncTasksFromTfsResponse>
{
    private readonly IMediator _mediator;

    private readonly MongoDBTfsIntegrationCollection _tfsIntegrationRepository;

    private readonly WorkItemAdapter _workItemAdapter;

    private readonly Repository _repository;

    public SyncTasksFromTfsHandler(
        IMediator mediator,
        MongoDBTfsIntegrationCollection tfsIntegrationRepository,
        Repository repository,
        WorkItemAdapter workItemAdapter)
    {
        _mediator = mediator;

        _tfsIntegrationRepository = tfsIntegrationRepository;

        _repository = repository;

        _workItemAdapter = workItemAdapter;
    }

    public async Task<SyncTasksFromTfsResponse> Handle(SyncTasksFromTfsRequest request, CancellationToken cancellationToken)
    {
        var tfsIntegrationList = await _tfsIntegrationRepository.GetTfsIntegrationList();

        foreach (var tfsIntegration in tfsIntegrationList)
        {
            var taskInfoList = await _workItemAdapter.GetTaskInfoList(tfsIntegration).ConfigureAwait(false);

            foreach (var taskInfo in taskInfoList)
            {
                var getTasksRequest = new GetTasksRequest
                {
                    ExternalReference = taskInfo.Reference
                };

                var getTasksResponse = await _mediator.Send<GetTasksResponse>(getTasksRequest);

                var taskQueryItem = getTasksResponse.TaskQueryItems.FirstOrDefault();

                TaskManagement.Model.Tasks.Task task;

                if (taskQueryItem == default)
                {
                    task = new TaskManagement.Model.Tasks.Task(taskInfo.Name);                    
                }
                else
                {
                    var taskExisting = await _repository.GetAggregateById<TaskManagement.Model.Tasks.Task>(taskQueryItem.Id);

                    if (taskExisting == null)
                    {
                        task = new TaskManagement.Model.Tasks.Task(taskInfo.Name);
                    }
                    else
                    {
                        task = taskExisting;

                        if (task.Description != taskInfo.Name)
                        {
                            task.ChangeDescription(taskInfo.Name);
                        }
                    }
                }

                await _repository.Save(task, -1);

                var taskIntegration = new TaskIntegration(task, taskInfo);

                await _repository.Save(taskIntegration, -1);
            }
        }

        var response = new SyncTasksFromTfsResponse(request.GetCorrelationId()) { };

        return response;
    }
}