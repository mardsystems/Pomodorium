Feature: Task Synchronizer

A short summary of the feature

Rule: TFS workitem system title should be translated to task description

@wip
Scenario: User requests synch tasks with TFS
	Given User starts a task synch with TFS
	When User request task synch
	Then System should get workitems from TFS. Examples:
		| system title |
		| Todo it A    |
		| Todo it B    |
		| Todo it C    |
	And System should create tasks
	And System should translates workitem system title to task description
	And task description should be equal to workitem system title
