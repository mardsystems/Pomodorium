using MediatR;
using Pomodorium.Trello;
using System.DomainModel;

namespace Pomodorium.Features.TaskManager;

public class SyncTasksWithTrelloHandler : IRequestHandler<SyncTasksWithTrelloRequest, SyncTasksWithTrelloResponse>
{
    private readonly IMediator _mediator;

    private readonly BoardsAdapter _boardsAdapter;

    private readonly ListsAdapter _listsAdapter;

    private readonly Repository _repository;

    public SyncTasksWithTrelloHandler(
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

    public async Task<SyncTasksWithTrelloResponse> Handle(SyncTasksWithTrelloRequest request, CancellationToken cancellationToken)
    {
        var lists = await _boardsAdapter.GetLists(request.BoardId).ConfigureAwait(false);

        foreach (var list in lists)
        {
            var cards = await _listsAdapter.GetCards(list.id).ConfigureAwait(false);

            foreach (var card in cards)
            {
                var getTasksRequest = new GetTasksRequest
                {
                    ExternalSourceId = card.id
                };

                var getTasksResponse = await _mediator.Send<GetTasksResponse>(getTasksRequest);

                var taskQueryItem = getTasksResponse.TaskQueryItems.FirstOrDefault();

                var cardTitle = $"{card.name}";

                TaskManagement.Model.Tasks.Task task;

                if (taskQueryItem == default)
                {
                    task = new TaskManagement.Model.Tasks.Task(cardTitle, card.id);
                }
                else
                {
                    var taskExisting = await _repository.GetAggregateById<TaskManagement.Model.Tasks.Task>(taskQueryItem.Id);

                    if (taskExisting == null)
                    {
                        task = new TaskManagement.Model.Tasks.Task(cardTitle, card.id);
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

        var response = new SyncTasksWithTrelloResponse(request.GetCorrelationId()) { };

        return response;
    }
}
