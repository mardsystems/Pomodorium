Feature: Settings

A short summary of the feature

Rule: TFS integration name should be required

@wip
Scenario: User settings a TFS integration with name filled
	Given User starts a TFS integration setting
	And User inputs TFS integration name as 'New TFS Project'
	And User inputs TFS integration project name as 'Project Abc'
	When User post TFS integration setting
	Then System should return TFS integration setting as expected
