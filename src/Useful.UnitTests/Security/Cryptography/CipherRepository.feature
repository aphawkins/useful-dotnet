Feature: CipherRepository
	In order to know all the ciphers
	As a cryptographer
	I want all the ciphers to be managed in one place

Scenario: CipherRepository - Create & Read
	Given I have a CipherRepository
	When I create a new cipher
	Then there should be "1" ciphers

Scenario: CipherRepository - Update
	Given I have a CipherRepository
	When I create a new cipher
	And I update a cipher
	Then there should be "1" ciphers

Scenario: CipherRepository - Delete
	Given I have a CipherRepository
	When I create a new cipher
	And I delete a cipher
	Then there should be "0" ciphers

Scenario: CipherRepository - SetCurrentItem
	Given I have a CipherRepository
	When I create a new cipher
	And I SetCurrentItem
	Then the CurrentItem will be set

Scenario: CipherRepository - SetCurrentItem null
	Given I have a CipherRepository
	When I SetCurrentItem
	Then the CurrentItem will not be set

Scenario: CipherRepository - Get Cipher Count
	Given I have a CipherRepository
	When I load the ciphers
	Then there should be "2" ciphers
