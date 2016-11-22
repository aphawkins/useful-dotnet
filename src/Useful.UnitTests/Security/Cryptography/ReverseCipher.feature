Feature: ReverseCipher
	In order to encrypt messages
	As a cryptographer
	I want to use the Reverse Cipher

Scenario: ReverseCipher - Ciphername
	Given I have a "Reverse" cipher
	Then the cipher name should be "Reverse"

Scenario: ReverseCipher - Encrypt a string
	Given I have a "Reverse" cipher
	And my plaintext is "Hello World"
	When I encrypt
	Then the ciphertext should be "dlroW olleH"

Scenario: ReverseCipher - Decrypt a string
	Given I have a "Reverse" cipher
	And my ciphertext is "dlroW olleH"
	When I decrypt
	Then the plaintext should be "Hello World"