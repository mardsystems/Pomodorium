Feature: Task Manager

Rule: Task name should be at least 2 characters

Scenario: Task registration with name longer than 1 character
	When Marcelo registers a task as
		| description |
		| Todo it     |
	Then the task should be registered as expected

Scenario: Task edit with name longer than 1 character
	Given that there is any customer
	When Marcelo registers a task as
		| description |
		| Todo it     |
	Then the task should be registered as expected
