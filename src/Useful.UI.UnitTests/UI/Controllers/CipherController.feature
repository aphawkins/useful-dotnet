Feature: CipherController

@mytag
Scenario: CipherController - Load the view
	Given I have a CipherController
	When I load the view
	Then the view's ShowCiphers will be called
	And the view will be initialized

Scenario: CipherController - Select a cipher
	Given I have a CipherController
	When I select a cipher
	Then the view's ShowCiphername will be called

Scenario: CipherController - Encrypt
	Given I have a CipherController
	When I encrypt "Plaintext"
	Then the view's Plaintext will be called
	And the view's Ciphertext will be called
