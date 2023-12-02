using MediatR;
using Pomodorium.Features.TaskManager;
using Pomodorium.Trello;
using System.DomainModel;

namespace Pomodorium.Features.TaskSynchronizer;

public class SyncTasksFromTrelloHandler : IRequestHandler<SyncTasksFromTrelloRequest, SyncTasksFromTrelloResponse>
{
    private readonly IMediator _mediator;

    private readonly BoardsAdapter _boardsAdapter;

    private readonly ListsAdapter _listsAdapter;

    private readonly Repository _repository;

    public SyncTasksFromTrelloHandler(
        IMediator mediator,
        Repository repository,
        BoardsAdapter boardsAdapter,
        ListsAdapter listsAdapter)
    {
        _mediator = mediator;

        _repository = repository;

        _boardsAdapter = boardsAdapter;

        _listsAdapter = listsAdapter;
    }

    public async Task<SyncTasksFromTrelloResponse> Handle(SyncTasksFromTrelloRequest request, CancellationToken cancellationToken)
    {
        var lists = await _boardsAdapter.GetLists(request.BoardId).ConfigureAwait(false);

        foreach (var list in lists)
        {
            var cards = await _listsAdapter.GetCards(list.id).ConfigureAwait(false);

            foreach (var card in cards)
            {
                var getTasksRequest = new GetTasksRequest
                {
                    ExternalReference = card.id
                };

                var getTasksResponse = await _mediator.Send<GetTasksResponse>(getTasksRequest);

                var taskQueryItem = getTasksResponse.TaskQueryItems.FirstOrDefault();

                var cardTitle = $"{card.name}";

                TaskManagement.Model.Tasks.Task task;

                if (taskQueryItem == default)
                {
                    task = new TaskManagement.Model.Tasks.Task(cardTitle); //card.id
                }
                else
                {
                    var taskExisting = await _repository.GetAggregateById<TaskManagement.Model.Tasks.Task>(taskQueryItem.Id);

                    if (taskExisting == null)
                    {
                        task = new TaskManagement.Model.Tasks.Task(cardTitle); //card.id
                    }
                    else
                    {
                        task = taskExisting;

                        if (task.Description != cardTitle)
                        {
                            task.ChangeDescription(cardTitle);
                        }
                    }
                }

                await _repository.Save(task, -1);
            }
        }

        var response = new SyncTasksFromTrelloResponse(request.GetCorrelationId()) { };

        return response;
    }
}
