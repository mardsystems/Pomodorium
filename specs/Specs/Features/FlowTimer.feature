Feature: Flow Timer

A short summary of the feature

Rule: Focus Timer

@wip
Scenario: Programmer focuses on a new task
	When programmer start focus on a new task
	And programmer start flowtime as:
	| task description |
	| Task A           |
	Then system should create a task
	And system should create a flowtime

	Given programmer was left with low productivity after 25 minutes
	When programmer stop flowtime
	Then system should suggest breaktime '5 minutes'

@wip
Scenario: Programmer focuses on an existing task
	Given that existing a task 'Task B'
	When programmer start focus on 'Task B'
	And programmer start flowtime
	Then system should create a flowtime

	Given programmer was left with low productivity after 25 minutes
	When programmer stop flowtime
	Then system should suggest breaktime '5 minutes'
