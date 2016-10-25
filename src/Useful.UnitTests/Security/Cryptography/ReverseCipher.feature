Feature: ReverseCipher
	In order to encrypt messages
	As a cryptographer
	I want to use the Reverse Cipher

@mytag
Scenario: ReverseCipher - Ciphername
	Given I have a "Reverse" cipher
	Then the cipher name should be "Reverse"

@mytag
Scenario: ReverseCipher - Encrypt a string
	Given I have a "Reverse" cipher
	And my plaintext is "Hello World"
	When I encrypt
	Then the ciphertext should be "dlroW olleH"