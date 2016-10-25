Feature: ROT13Cipher
	In order to encrypt messages
	As a cryptographer
	I want to use the ROT13 Cipher

@mytag
Scenario: ROT13Cipher - Ciphername
	Given I have a "ROT13" cipher
	Then the cipher name should be "ROT13"

@mytag
Scenario: ROT13Cipher - Encrypt a string
	Given I have a "ROT13" cipher
	And my plaintext is "Hello World"
	When I encrypt
	Then the ciphertext should be "Uryyb Jbeyq"