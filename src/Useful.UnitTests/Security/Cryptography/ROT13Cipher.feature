Feature: ROT13Cipher
	In order to encrypt messages
	As a cryptographer
	I want to use the ROT13 Cipher

Scenario: ROT13Cipher - Ciphername
	Given I have a 'ROT13' cipher
	Then the cipher name should be 'ROT13'

Scenario Outline: ROT13Cipher - Encrypt a string
	Given I have a 'ROT13' cipher
	And my plaintext is <plaintext>
	When I encrypt
	Then the ciphertext should be <ciphertext>
	Examples:
	| plaintext                  | ciphertext                 |
	| ABCDEFGHIJKLMNOPQRSTUVWXYZ | NOPQRSTUVWXYZABCDEFGHIJKLM |
	| abcdefghijklmnopqrstuvwxyz | nopqrstuvwxyzabcdefghijklm |
	| >?@ [\]                    | >?@ [\]                    |

Scenario Outline: ROT13Cipher - Decrypt a string
	Given I have a 'ROT13' cipher
	And my ciphertext is <ciphertext>
	When I decrypt
	Then the plaintext should be <plaintext>
	Examples:
	| plaintext                  | ciphertext                 |
	| ABCDEFGHIJKLMNOPQRSTUVWXYZ | NOPQRSTUVWXYZABCDEFGHIJKLM |
	| abcdefghijklmnopqrstuvwxyz | nopqrstuvwxyzabcdefghijklm |
	| >?@ [\]                    | >?@ [\]                    |