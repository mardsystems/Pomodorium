Feature: Task Synchronizer

A short summary of the feature

Background:
	Given exists a TFS integration settings as
		| name        | project name |
		| TFS Project | Project A    |

Rule: TFS workitem system title should be translated to task description

Scenario: User requests synch tasks with TFS
	Given User starts a task synch with TFS
	And exists a workitems in TFS as
		| system title |
		| Todo it A    |
		| Todo it B    |
		| Todo it C    |
	When User request task synch with TFS
	Then System should get workitems from TFS
	And System should create tasks as
		| description    |
		| Todo it A (#1) |
		| Todo it B (#2) |
		| Todo it C (#3) |
	#And System should translates workitem system title to task description
	#And task description should be equal to workitem system title
