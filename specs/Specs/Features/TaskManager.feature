Feature: Task Manager

Rule: Task description should be at least 2 characters

Scenario: User registers task with description longer than 1 character
	Given User starts a task registration
	And User inputs task description as 'Todo it'
	When User register task
	Then System should create a task as expected

Scenario: User changes task description with longer than 1 character
	Given User starts a change task description
	And User inputs task description as 'Todo changed'
	When User change task description
	Then System should change task description as expected
