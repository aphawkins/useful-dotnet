Feature: CipherRepository
	In order to know all the ciphers
	As a cryptographer
	I want all the ciphers to be managed in one place

@mytag
Scenario: CipherRepository - Get Cipher Count
	Given I have a CipherRepository
	Then there should be "2" ciphers
