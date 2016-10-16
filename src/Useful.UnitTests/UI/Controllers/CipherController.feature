Feature: CipherController

@mytag
Scenario: CipherController - Load the view
	Given I have a CipherController
	When I load the view
	Then the view's CipherName will be called
	And the view will be initialized

Scenario: CipherController - Encrypt
	Given I have a CipherController
	When I encrypt "Plaintext"
	Then the view's Plaintext will be called
	And the view's Ciphertext will be called
