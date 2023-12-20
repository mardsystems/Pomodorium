Feature: Task Manager

Rule: Task name should be at least 2 characters

Scenario: Task registration with description longer than 1 character
	When Programmer registers a task 'Todo it'
	Then the task should be registered as expected

@wip
Scenario: Change task description with longer than 1 character
	Given that there is any customer
	When Programmer change a task to 'Todo it'
	Then the task should be registered as expected
