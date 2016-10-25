Feature: RelayCommand

@mytag
Scenario: RelayCommand - Execute
	Given I have a RelayCommand with a handler that I can execute
	And I can execute
	When I execute 
	Then the handler will get executed

Scenario: RelayCommand - Not Execute
	Given I have a RelayCommand with a handler that I can't execute
	And I can't execute
	When I execute 
	Then the handler will not get executed

Scenario: RelayCommand - No Execute
	Given I have a RelayCommand with a null handler
	Then the RelayCommand constructor will exception